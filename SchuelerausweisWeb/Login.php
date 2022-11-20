<?php

function ConnectLdap()
{
    try {
        $ldapConn = ldap_connect("localhost", 389)
            or die("That LDAP-URI was not parseable");

        if (!$ldapConn) {
            echo ("Es konnte keine Verbindung hergestellt werden.");
            throw new Exception();
        }

        ldap_set_option($ldapConn, LDAP_OPT_PROTOCOL_VERSION, 3);

        $ldapBind = ldap_bind($ldapConn, "cn=" . $_POST['usr'] . ",dc=yourOrganisation,dc=loc", $_POST['pwd']);

        if (!$ldapBind) {
            echo "Passwort stimmt nicht überein.";
            throw new Exception();
        }

        return $ldapConn;
    } catch (Exception $e) {
        echo ("Der Benutzer konnte nicht angemeldet werden.");
        return null;
    }
}

function GetLdapId($ldapConn)
{
    try {
        $ldapSearch = ldap_search($ldapConn, "cn=" . $_POST['usr'] . ",dc=yourOrganisation,dc=loc", "(&(objectClass=inetOrgPerson))");
        $ldapEntry = ldap_first_entry($ldapConn, $ldapSearch);
        $ldapValues = ldap_get_values($ldapConn, $ldapEntry, "displayname");
        return $ldapValues[0];
    } catch (Exception $e) {
        echo ("Es konnte keine Id geladen werden.");
        return null;
    }
}

error_reporting(0);
ini_set('display_errors', 0);

try {
    $ldapConn = ConnectLdap();
    $id = GetLdapId($ldapConn);

    include("QR.php");
    GeneriereQR($id);
} catch (Exception $e) {
    throw new Exception("Error Processing Request");
}
