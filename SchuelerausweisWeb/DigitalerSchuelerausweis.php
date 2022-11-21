<?php
function ErstelleSchuelerausweis($schuelerDaten)
{
    $vorlage = imagecreatefrompng('img/SchuelerausweisVorlage.png');
    $ausweis = AusweisMitDatenFuellen($vorlage, $schuelerDaten);

    ob_clean();
    header('Content-Type: image/png');

    imagepng($ausweis);
    imagedestroy($ausweis);
}

function AusweisMitDatenFuellen($vorlage, $schuelerDaten)
{
    $date = new DateTimeImmutable($schuelerDaten['dateOfBirth']);

    imagettftext($vorlage, 20, 0, 340, 200, 0x000000, 'img/Arial.ttf', $schuelerDaten['firstName']);
    imagettftext($vorlage, 20, 0, 340, 250, 0x000000, 'img/Arial.ttf', $schuelerDaten['lastName']);
    imagettftext($vorlage, 20, 0, 340, 360, 0x000000, 'img/Arial.ttf', $date->format("d.m.Y"));
    imagettftext($vorlage, 20, 0, 340, 480, 0x000000, 'img/Arial.ttf', $schuelerDaten['class']);
    imagettftext($vorlage, 20, 0, 640, 480, 0x000000, 'img/Arial.ttf', "2020");

    $bildpfadDebug = "D:\\temp\\profilfoto\\luca.png";

    $foto = imagecreatefrompng($bildpfadDebug);
    $fotoSize = getimagesize($bildpfadDebug);
    imagecopyresized($vorlage, $foto, 50, 170, 0, 0, 250, 320, $fotoSize[0], $fotoSize[1]);

    return $vorlage;
}

function GetDataFromWebApi($token)
{
    $ch = curl_init();

    curl_setopt($ch, CURLOPT_URL, getenv('WEB_API_PATH'));
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($ch, CURLOPT_POST, 1);
    curl_setopt($ch, CURLOPT_POSTFIELDS, "\"" . $token . "\"");

    $headers = array();
    $headers[] = 'Accept: */*';
    $headers[] = 'Content-Type: text/json';
    
    curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);

    $result = curl_exec($ch);
    if (curl_errno($ch)) {
        echo 'Error:' . curl_error($ch);
    }

    curl_close($ch);

    return $result;
}

$schuelerDaten = GetDataFromWebApi(urlencode($_GET['t']));
if (!$schuelerDaten) {
    echo "<br>Es konnte kein valider Schülerausweis gefunden werden.";
} else {
    ErstelleSchuelerausweis(json_decode($schuelerDaten, true));
}
