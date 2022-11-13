<?php

$usr = $_POST['usr'];
$pwd = $_POST['pwd'];



// make sure your host is the correct one
// that you issued your secure certificate to
$ldaphost = "localhost:8081";

// Connecting to LDAP
$ldapconn = ldap_connect($ldaphost)
          or die("That LDAP-URI was not parseable");



?>