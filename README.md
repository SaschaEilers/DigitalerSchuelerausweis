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

### Planungsphase

Zuerst wurde das Projekt geplant, wie man was umsetzen kann.

#### UML-Diagramme

Zur Planung des Projekts wurde ein Anwendungsfalldiagramm erstellt:

* [Use Case Diagramm](Dokumente/Diagramme/UseCase.pdf)

Außerdem wurde für die verfeinerte Planung des Web Services und der Web API jeweils ein Klassendiagramm erstellt, um so die Verbindungen zwischen den unterschiedlichen Klassen und Objekten aufzuzeigen:

* [Klassendiagramm - Web Service](Dokumente/Diagramme/Klassendiagramm_WebService.pdf)
* [Klassendiagramm - Web API](Dokumente/Diagramme/Klassendiagramm_WebApi.pdf)

Die UML-Diagramme wurden während der Projektdurchführung aktualisiert und auf den neuesten Stand gebracht.

#### Oberfläche

Um auch die einzelnen Oberflächen, sowie den Übergang zwischen diesen Oberflächen besser zu planen, wurden sogenannte Wireframes erstellt:

* [Schritt 1 - Log In](Dokumente/Wireframe/1_LogIn.pdf)
* [Schritt 2 - Log In Fenster](Dokumente/Wireframe/2_QR.pdf)
* [Schritt 3 - Ausweis](Dokumente/Wireframe/3_Ausweis.pdf)

### Teamrollen

* Sascha Eilers
  * Projektorganisation
  * Wireframes für die Oberfläche
  * Web Service
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

### Verwendete Ressourcen

* GitHub
* Visual Studio Code
* JetBrains Rider
* Docker
* UMLet
* Microsoft Office
