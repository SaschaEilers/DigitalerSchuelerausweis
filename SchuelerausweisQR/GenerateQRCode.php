<?php
include "\packages\phpqrcode\phpqrcode\qrlib.php";

$_POST['usr'];
$link = "Test";
$Timestamp = time();

$token = $Timestamp + $_POST;
$text = $link + $token;

$test = "https://www.google.com/"
?>

<div>
    <?php
    QRcode::($test);
    ?>
</div>
