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
        setUp();
        $_POST array(
            'usr' => "Test"
            'pwd' => $pwd
        )
    }

    public function testConnectLdapTrue(): void{
        // Vorbereitung
        vorbereitung("TestPasswort");

        // Methodenaufruf ConnectLdap
        $connection = ConnectLdap();

        // Ergebnis
        $this -> assertNotSame($connection, null)
    }

    public function testConnectLdapFalse(): void{
        //Vorbereitung
        vorbereitung("FalschesTestPasswort")

        // Methodenaufruf COnnectLdap
        $connection = ConnectLdap();

        // Ergebnis
        $this -> assertSame($connection, null)
    }
}