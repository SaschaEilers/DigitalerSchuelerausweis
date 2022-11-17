

<?php

function ConnectLdap()
{
    try {
        $ldapConn = ldap_connect("localhost", 389)
            or die("That LDAP-URI was not parseable");

        if (!$ldapConn) {
            throw new Exception();
        }

        ldap_set_option($ldapConn, LDAP_OPT_PROTOCOL_VERSION, 3);

        $ldapBind = ldap_bind($ldapConn, "cn=user-read-only,dc=yourOrganisation,dc=loc", "user-read-only");

        if (!$ldapBind) {
            throw new Exception();
        }

        return $ldapConn;
    } catch (Exception $e) {
        echo ("Es konnte keine Connection zu LDAP hergestellt werden." . $e);
        return null;
    }
}

function GetAnmeldeDaten($ldapConn, $usr, $pwd)
{
    try {

        $ldapSearchLogIn = ldap_search($ldapConn, "cn=" . $usr . ",dc=yourOrganisation,dc=loc", "(&(objectClass=inetOrgPerson))");
        $ldapEntryLogIn = ldap_first_entry($ldapConn, $ldapSearchLogIn);
        $ldapValues = ldap_get_values($ldapConn, $ldapEntryLogIn, "userPassword");
        if ($pwd == $ldapValues[0])
        {
            return;
        }
        echo "Passwort stimmt nicht überein.";
    } catch (Exception $e) {
        echo "Benutzer konnte nicht gefunden werden." . $e;
        return null;
    }
}

try {
    $usr = $_POST['usr'];
    $pwd = $_POST['pwd'];

    $ldapConn = ConnectLdap();
    GetAnmeldeDaten($ldapConn, $usr, $pwd);

    $ldapSearch = ldap_search($ldapConn, "cn=sascha,dc=yourOrganisation,dc=loc", "(&(objectClass=inetOrgPerson))");
    $ldapEntry = ldap_first_entry($ldapConn, $ldapSearch);

    $ldapValues = ldap_get_values($ldapConn, $ldapEntry, "Email");
    $id = $ldapValues[0];

    echo "<h1>" . $id . "</h1>";
} catch (Exception $e) {
    throw new Exception("Error Processing Request" . $e);
}
