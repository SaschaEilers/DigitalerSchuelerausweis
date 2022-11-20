<?php
function EncryptSchueler($text)
{
    $algo = "aes-128-cbc";
    $key = "DigitalerSchülerausweis";
    $iv = "1234567891011112";

    return openssl_encrypt($text, $algo, $key, 0, $iv);
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
