<?php
function EncryptSchueler($text)
{
    return openssl_encrypt($text, getenv('ENCRYPTION_ALGO'), getenv('ENCRYPTION_KEY'), 0, getenv('ENCRYPTION_IV'));
}

function GeneriereQR($id)
{
    include "./package/phpqrcode.php";

    $pass = date("Y-m-d\TH:i:s") . "|cn=" . $id . "|";
    $cryptPass = EncryptSchueler($pass);
    $urlCryptPass = urlencode($cryptPass);
    $url = $_SERVER["REQUEST_SCHEME"] . "://". $_SERVER["SERVER_NAME"] .":" . $_SERVER["SERVER_PORT"] . "/DigitalerSchuelerausweis.php?t=";

    QRcode::png($url . $urlCryptPass);
}
