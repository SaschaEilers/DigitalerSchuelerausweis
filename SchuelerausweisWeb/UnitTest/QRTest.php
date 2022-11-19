<?php
use PHPUnit\Framework\TestCase;

final class QRTest extends TestCase
{
    public function testCreateEncryption(): void{
        
        // Vorbereitung
        include_once('../QR.php');
        
        $datum = date("Y-m-d H:i:s");
        $id = "Sascha";
        $text = $datum.'|cn='.$id;

        // Methodenaufruf Encrypt
        $encrypt = EncryptSchueler($text);

        // Decrypt
        $algo = "aes-128-cbc";
        $key = "Schülerausweis";
        $iv = '1234567891011112';

        $decrypt = openssl_decrypt($encrypt,$algo, $key, 0, $iv);
        
        // Ergebnis
        $string = explode("|",$decrypt);

        $name = $string[1];
        $this -> assertSame($id, "cn=".$name);

        $date = DateTime::createFromFormat('Y-m-d H:i:s',$string[0]);
        $this -> assertSame($datum, $date);
    }

    public function testEncryptionWorks(): void{

        // Vorbereitung
        include_once('../QR.php');
        
        $datum = date("Y-m-d H:i:s");
        $id = "Sascha";
        $text = $datum.'|cn='.$id;

        // Methodenaufruf Encrypt
        $encrypt = EncryptSchueler($text);


        $this -> assertNotSame($encrypt, $text);
    }
}