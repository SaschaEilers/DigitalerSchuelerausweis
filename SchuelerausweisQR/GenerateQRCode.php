<?php
function EncryptSchueler(){
    $simple_string = "Luca Brandt";
  
// Display the original string
//echo "Original String: " . $simple_string;
  
// Store the cipher method
$ciphering = "aes128";//"AES-128-CTR";
  
// Use OpenSSl Encryption method
$iv_length = openssl_cipher_iv_length($ciphering);
$options = 0;
  
// Non-NULL Initialization Vector for encryption
$encryption_iv = '1234567891011112';
  
// Store the encryption key
$encryption_key = "GeeksforGeeks";
  
// Use openssl_encrypt() function to encrypt the data
$encryption = openssl_encrypt($simple_string, $ciphering,
            $encryption_key, $options, $encryption_iv);

            return $encryption;
}

include ".\phpqrcode\qrlib.php";

$schueler = "Luca";
//$schuelerhash = $schueler.openssl_encrypt('ripemd160', $schueler);
$schuelerhash = EncryptSchueler();
$link = "Test";
$Timestamp = time();
//var_dump($schuelerhash);
$token = $Timestamp ;
// $text = $link
$tesssst = urlencode($schuelerhash);
$test = "www.google.com/search?q=".$tesssst;
//echo $test;
   QRcode::png($test);
    
    ?>

