<?php
use PHPUnit\Framework\TestCase;

final class LoginTest extends TestCase
{
    protected function setUp()
    {
        parent::setUp();
        $_POST = array();
    }

    public function vorbereitung($pwd): array{
        $this->setUp();
        $_POST = array(
            'usr' => "Test",
            'pwd' => $pwd
        );
    }

    public function testConnectLdapTrue(): void{
        // Vorbereitung
        $this->vorbereitung("TestPasswort");

        // Methodenaufruf ConnectLdap
        $connection = ConnectLdap();

        // Ergebnis
        $this -> assertNotSame($connection, null);
    }

    public function testConnectLdapFalse(): void{
        //Vorbereitung
        $this->vorbereitung("FalschesTestPasswort");

        // Methodenaufruf ConnectLdap
        $connection = ConnectLdap();

        // Ergebnis
        $this -> assertSame($connection, null);
    }

    public function testGetLdapId(): void{
        //Vorbereitung
        $this->vorbereitung("TestPaswort");
        $ldapConn = ConnectLdap();

        // Methodenaufruf GetLdapId
        $ldapValues = GetLdapId($ldapConn);

        //Ergebnis
        $this -> assertNotSame($ldapValues, null);
    }
}