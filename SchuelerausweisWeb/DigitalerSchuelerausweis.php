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
    imagettftext($vorlage, 20, 0, 340, 200, 0x000000, 'img/Arial.ttf', $schuelerDaten['Vorname']);
    imagettftext($vorlage, 20, 0, 340, 250, 0x000000, 'img/Arial.ttf', $schuelerDaten['Nachname']);
    imagettftext($vorlage, 20, 0, 340, 360, 0x000000, 'img/Arial.ttf', $schuelerDaten['Geburtstag']);
    imagettftext($vorlage, 20, 0, 340, 480, 0x000000, 'img/Arial.ttf', $schuelerDaten['Klasse']);
    imagettftext($vorlage, 20, 0, 640, 480, 0x000000, 'img/Arial.ttf', "2020");

    $foto = imagecreatefrompng($schuelerDaten['BildPfad']);
    $fotoSize = getimagesize($schuelerDaten['BildPfad']);
    imagecopyresized($vorlage, $foto, 50, 170, 0, 0, 250, 320, $fotoSize[0], $fotoSize[1]);

    return $vorlage;
}

function GetDataFromWebApi()
{

    $url = "http://localhost:12345/api/Values/Get";

    $ch = curl_init();

    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($ch, CURLOPT_CONNECTTIMEOUT, 4);

    $json = curl_exec($ch);

    $date = new DateTimeImmutable("2001-06-21");
    $debug = array(
        'Vorname' => "Sascha",
        'Nachname' => "Eilers",
        'Klasse' => "WIT3C",
        'Geburtstag' => $date->format("d.m.Y"),
        'BildPfad' => "D:\\temp\\profilfoto\\sascha.png"
    );

    return $debug;
}

$schuelerDaten = GetDataFromWebApi();
if (!$schuelerDaten) {
    echo "Es konnte kein valider Schülerausweis gefunden werden.";
} else {
    ErstelleSchuelerausweis($schuelerDaten);
}
