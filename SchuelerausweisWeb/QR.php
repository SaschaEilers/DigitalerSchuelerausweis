<?php
function EncryptSchueler($text)
{
    return openssl_encrypt($text, $_ENV['ENCRYPTION_ALGO'], $_ENV['ENCRYPTION_KEY'], 0, $_ENV['ENCRYPTION_IV']);
}

function GeneriereQR($id)
{
    include ".\package\phpqrcode.php";

    $pass = date("Y-m-d\TH:i:s") . "|cn=" . $id . "|";
    $cryptPass = EncryptSchueler($pass);
    $urlCryptPass = urlencode($cryptPass);
    $url = $_SERVER["REQUEST_SCHEME"] . "://". $_SERVER["SERVER_NAME"] .":" . $_SERVER["SERVER_PORT"] . "/DigitalerSchuelerausweis.php?t=";

    QRcode::png($url . $urlCryptPass);
}
