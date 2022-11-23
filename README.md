# Schulprojekt: Digitaler Schülerausweis

## Schritt für Schritt Anleitung

1. `docker compose up`
2. LDAP einrichten
   1. Anmeldedaten:
      * cn=admin,dc=yourOrganisation,dc=loc
      * admin
   2. Create new entry here
      1. Default
      2. inetOrgPerson
         * RDN - cn
         * cn - Vorname
         * sn - Nachname
         * title - Geburtstag (yyyy-MM-dd)
         * roomNumber - Klasse
         * departmentNumber - Einschulungsjahr
         * labeledUri - Bilddatei
         * Password - Passwort
3. Website öffnen
   * IP Adresse des lokalen PCs + Port
   * xxx.xxx.xxx.xxx:9000
4. Mit einem Schüler einloggen
5. QR Code mit einem Smartphone scannen
6. Schülerausweis anzeigen lassen

### Konfigurationsmöglichkeiten

In der [Docker Compose yaml](docker-compose.yml) können verschiedene Variablen konfiguriert werden.

* backend - environment:
  * TokenLifeSpan
    * Hier kann die Dauer eingestellt, wie lange der Token aktiv ist.
  * Attributes
    * Das sind die Attribute aus LDAP die dann auf die Schülerdaten gemappt werden.
* frontend - environment
  * IMG_DIR
    * Der Pfad zu dem Ordner, wo die Bilder der Schüler gespeichert sind.

## Projektdurchführung

### Aufgabenzuteilung

Die Aufgaben wurden vor Projektbeginn unter dem Team aufgeteilt und zum Schluss in mehreren gemeinsamen Besprechungen zusammen besprochen und zusammengefügt:

* [Aufgabenverteilung](TODO.md "TODO Liste, die die Augabenverteilung im Team aufzeigt")

### UML-Diagramme

Zur Planung des Projekts wurde ein Anwendungsfalldiagramm erstellt:

* [Use Case Diagramm](Dokumente/Diagramme/UseCase.pdf "Anwendungfalldiagramm, für die Planung des Projekts")

Außerdem wurde für die verfeinerte Planung des Web Services und der Web API jeweils ein Klassendiagramm erstellt, um so die Verbindungen zwischen den unterschiedlichen Klassen und Objekten aufzuzeigen:

* [Klassendiagramm - Web Service](Dokumente/Diagramme/Klassendiagramm_WebService.pdf "Planung der Klassen des Web Services")
* [Klassendiagramm - Web API](Dokumente/Diagramme/Klassendiagramm_WebApi.pdf "Planung der Klassen für die Web API")

Die UML-Diagramme wurden während der Projektdurchführung aktualisiert und auf den neuesten Stand gebracht.

### Oberfläche

Um auch die einzelnen Oberflächen, sowie den Übergang zwischen diesen Oberflächen besser zu planen, wurden sogenannte Wireframes erstellt:

* [Schritt 1 - Log In](Dokumente/Wireframe/1_LogIn.pdf "1. Schitt - Login Fenster")
* [Schritt 2 - QR Code](Dokumente/Wireframe/2_QR.pdf "2. Schritt - QR Code Anzeige für den Schüler")
* [Schritt 3 - Ausweis](Dokumente/Wireframe/3_Ausweis.pdf "3.Schritt - Angezeigter Schülerausweis")

## Ressourcen

### Teamrollen

* Sascha Eilers
  * Projektorganisation
  * Wireframes für die Oberfläche
  * Web Service
  * Klassendiagramm
* Obed Kooijmann
  * Web API
  * Web API Tests
  * Einrichtung LDAP
  * Einrichtung Docker
* Luca Brandt
  * Unterstützung Web Service
  * Web Service Tests
  * Anwendungsfalldiagramm
  * Klassendiagramm

### Verwendete Software

* GitHub
* Visual Studio Code
* JetBrains Rider
* Docker
* UMLet
* Microsoft Office
