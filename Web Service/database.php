<?php

class databaseHandler
{
    private $database_host = 'localhost';
    private $database_name = '';
    private $database_user = '';
    private $database_pass = '';
    public $database = null;

    // PRIVATE DATABASE FUNCTIONS
    public function db_connect()
    {
        if ($this->database == null) {
            try {
                $this->database = new PDO("mysql:host={$this->database_host};dbname={$this->database_name}", $this->database_user, $this->database_pass);
            } catch (PDOException $e) {
                print "Unable to connect: " . $e->getMessage() . "<br/>";
                die();
            }
        }
    }

    public function db_disconnect()
    {
        $this->database = null;
    }

    // PUBLIC CUSTOM DATABASE FUNCTIONS
    // Notams
    public function addNotam(string $reference, string $meaning, string $suffix, array $centralMarker, array $surroundingPolypoints, $rad = null)
    {
        // Basic input validation
        if (empty($reference) | empty($surroundingPolypoints) | empty($centralMarker["lat"]) | empty($centralMarker["lng"])) throw new Exception("Reference/polypoints cannot be null.");
        // Check if reference already exists
        $this->db_connect();
        try {
            $checkExists = $this->database->prepare("SELECT id FROM notam WHERE ntReference = ?");
            $checkExists->execute(array($reference));
            if ($checkExists->rowCount() == 0) {
                // Reference does not exist, create it!
                $createNotam = $this->database->prepare("INSERT INTO notam (ntReference, ntMeaning, ntSuffix, lat, lon, id) VALUES (?, ?, ?, ?, ?, default)");
                $createNotam->execute(array($reference, $meaning, $suffix, $centralMarker["lat"], $centralMarker["lng"]));
                $notamId = $this->database->lastInsertId();
                foreach ($surroundingPolypoints as $value) {
                    $createPolypoint = $this->database->prepare("INSERT INTO notam_polypoints (lat, lon, rad, refId, id) VALUES (?, ?, ?, ?, default)");
                    $createPolypoint->execute(array($value[0], $value[1], $rad, $notamId));
                }
                return true;
            } else {
                // Reference already exists..
                return false;
            }
        } catch (PDOException $e) {
            throw new Exception("Database error: " . $e->getMessage());
        }
    }

    public function deleteNotam(int $id)
    {
        // Basic input validation
        if (empty($id)) throw new Exception("Identification cannot be null.");
        // Check if notam exists
        $this->db_connect();
        try {
            $checkExists = $this->database->prepare("SELECT id FROM notam WHERE id = ?");
            $checkExists->execute(array($id));
            if ($checkExists->rowCount() > 0) {
                // We have a notam!
                // Let's delete it
                $deleteNotam = $this->database->prepare("DELETE FROM notam WHERE id = ?");
                $deleteNotam->execute(array($id));
                // And now its associated polypoints
                $deletePoints = $this->database->prepare("DELETE FROM polypoints WHERE refId = ?");
                $deletePoints->execute(array($id));
                return true;
            } else {
                // No notam..
                return false;
            }
        } catch (PDOException $e) {
            throw new Exception("Database error: " . $e->getMessage());
        }
    }

    // Airspace
    public function addAirspace(string $category, string $name, string $type, string $flLower, string $flUpper, array $polypoints)
    {
        // Basic input validation
        if (empty($name) | empty($polypoints)) throw new Exception("Name/polypoints cannot be null.");
        // Check if airspace already exists (using name)
        $this->db_connect();
        try {
            // Clean the name value.. (typically surrounded in html tags)
            $matches = array();
            $name = preg_match("#<b>(.*?)</b>#", $name, $matches);
            $name = $name[1];
            $checkExists = $this->database->prepare("SELECT id FROM airspace WHERE name = ?");
            $checkExists->execute(array($name));
            if ($checkExists->rowCount() == 0) {
                // Name (and thus airspace) does not exist, create it!
                $createAirspace = $this->database->prepare("INSERT INTO airspace (airCategory, airName, airType, flLower, flUpper, id) VALUES (?, ?, ?, ?, ?, default)");
                $createAirspace->execute(array($category, $matches[1], $type, str_replace("FL", "", $flLower), str_replace("FL", "", $flUpper)));
                $airspaceId = $this->database->lastInsertId();
                foreach ($polypoints as $value) {
                    $createPolypoint = $this->database->prepare("INSERT INTO airspace_polypoints (lat, lon, refId, id) VALUES (?, ?, ?, default)");
                    $createPolypoint->execute(array($value[0], $value[1], $airspaceId));
                }
                return true;
            } else {
                // Reference already exists..
                return false;
            }
        } catch (PDOException $e) {
            throw new Exception("Database error: " . $e->getMessage());
        }
    }

    public function deleteAirspace(int $id)
    {
        // Basic input validation
        if (empty($id)) throw new Exception("Identification cannot be null.");
        // Check if airspace exists
        $this->db_connect();
        try {
            $checkExists = $this->database->prepare("SELECT id FROM airspace WHERE id = ?");
            $checkExists->execute(array($id));
            if ($checkExists->rowCount() > 0) {
                // We have a airspace!
                // Let's delete it
                $deleteAirspace = $this->database->prepare("DELETE FROM airspace WHERE id = ?");
                $deleteAirspace->execute(array($id));
                // And now its associated polypoints
                $deletePoints = $this->database->prepare("DELETE FROM polypoints WHERE refId = ?");
                $deletePoints->execute(array($id));
                return true;
            } else {
                // No airspace..
                return false;
            }
        } catch (PDOException $e) {
            throw new Exception("Database error: " . $e->getMessage());
        }

    }

}
