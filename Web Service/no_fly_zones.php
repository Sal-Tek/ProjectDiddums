<?php
require_once("decodePolylineToArray.php");
require_once("database.php");
// Default max execution time is 30 seconds so let's give it a little longer
ini_set('max_execution_time', 300);
// Predefined global variables
$sources = array(
    "UK" => array(
        "Notams" => true,
        "Airspace" => true
    ),
    "US" => array(
        "Notams" => false
    )
); // Enable/disable scraping for sources
$debug = false; // Debugging?
$databaseHandler = new databaseHandler(); // Get our database handler
// Basic authentication
if (md5($_GET['auth']) != "3a486efffb9b7913be8b7422d1b119d1") die("No.");
// UK Data
// - Functions
// -- This part could do with some optimisation but it works within time constraints for now.#
function recursiveParameterGrab($prefix, $source)
{
    $initialMatches = array();
    $detailedMatches = array();
    preg_match_all('/' . preg_quote("$prefix(") . '(.*?)' . preg_quote(");") . '/is', $source, $initialMatches);
    if (empty($initialMatches[0]) || empty($initialMatches[1])) throw new Exception("Unexpected result from preg_match.");
    foreach ($initialMatches[1] as $value) {
        if ($value[0] == "\"") {
            // We have a supposedly valid result!
            $detailedMatches[] = str_getcsv($value, ",", '"');
        }
    }
    return $detailedMatches;
}
// - Call our sources
if($sources["UK"]["Notams"] | $sources["UK"]["Airspace"]) {
    try {
        $notamSource = file_get_contents('http://notaminfo.com/ukmap');
        if ($notamSource === false) throw new Exception("Unable to query target website. Check your internet connection.");
        $notams = recursiveParameterGrab("notam", $notamSource);
        $airspace = recursiveParameterGrab("airspace", $notamSource);
        foreach ($notams as $value) {
            if (count($value) == 19) $databaseHandler->addNotam($value[4], $value[5], $value[7], array("lat" => $value[1], "lng" => $value[2]), decodePolylineToArray($value[14]));
        }
        foreach ($airspace as $value) {
            if (count($value) == 13) $databaseHandler->addAirspace($value[0], $value[8], $value[9], $value[10], $value[11], decodePolylineToArray($value[2]));
        }
        $databaseHandler->db_disconnect();
    } catch (Exception $e) {
        if ($debug)
            print("Exception: " . $e->getMessage());
        else
            die("An error occurred.");
    }
}

// US Data
// - Functions
function convertDMtoDecimal($x, $y, $z)
{
    return round(((float)($x + (($y . "." . $z)) / 60)), 6);
}
function gatherUSNOTAMURLS(string $url, $websiteContents)
{
    if (filter_var($url, FILTER_VALIDATE_URL) === FALSE) throw new Exception("Invalid URL.");
    $IDMatches = array();
    preg_match_all('/..\/save_pages\/detail_6_(....).html/', $websiteContents, $IDMatches);
    $IDMatches = array_unique($IDMatches[1]);
    if (count($IDMatches) == 0) throw new Exception("No ID matches with URL: " . $url);
    return $IDMatches;
}
// - Call our sources
if($sources["US"]["Notams"]) {
    try {
        $USNotamURL = "http://tfr.faa.gov/tfr2/list.html";
        $USNotamSource = file_get_contents($USNotamURL);
        $USNotamIDs = gatherUSNOTAMURLS($USNotamURL, $USNotamSource);
        foreach ($USNotamIDs as $value) {
            if (!is_numeric($value)) {
                throw new Exception("Invalid NOTAM detected: " . $value);
            } else {
                // REQUIRES OPTIMISATION
                // MATCH ARRAYS
                $matches = array();
                $GPSData = array();
                // MAKE THE SOURCE CODE A SINGLE LINE
                $NOTAM = str_replace(array("\r\n", "\r", "\n"), "", file_get_contents("http://tfr.faa.gov/save_pages/detail_6_$value.html"));
                $NOTAM_data = array();
                // IDENTIFICATION
                preg_match("/\<title\>(.*) NOTAM Details\<\/title\>/", $NOTAM, $matches);
                $NOTAM_data["Reference"] = $matches[1];
                // REASON FOR NOTAM
                preg_match("/\<font face\=\"Arial\" size\=\"2\"\>Reason for NOTAM     \:\<\/font\>    \<\/TD\>    \<TD\>      \<font face\=\"Arial\" size\=\"2\"\>(.*?)<\/font>/", $NOTAM, $matches);
                $NOTAM_data["Reason"] = (!empty($matches[1])) ? $matches[1] : "No reason found.";
                // RADIUS
                preg_match("/\<font face\=\"Arial\" size\=\"2\"\>Radius\:\<\/font\>    \<\/TD\>    \<TD\>      \<font face\=\"Arial\" size\=\"2\"\>(.*?)<\/font>/", $NOTAM, $matches);
                preg_match("/\d+/", $matches[1], $matches);
                $NOTAM_data["Radius"] = (!empty($matches[0])) ? $matches[0] : 5;
                // GPS DATA
                preg_match("/\(Latitude\: (\d+)&#xBA;(\d+)'(\d+)\"\S, Longitude\: (\d+)&#xBA;(\d+)'(\d+)\S\S\)/", $NOTAM, $GPSData);
                $NOTAM_data["GPS"]["Latitude"] = convertDMtoDecimal($GPSData[1], $GPSData[2], $GPSData[3]);
                $NOTAM_data["GPS"]["Longitude"] = convertDMtoDecimal($GPSData[4], $GPSData[5], $GPSData[6]);
                // ADD IT TO THE DATABASE
                // - ONLY IF WE HAVE ALL THE DATA
                if (!(empty($NOTAM_data['Reference']) | empty($NOTAM_data['Reason']) | empty($NOTAM_data['Radius']) | $NOTAM_data["GPS"]["Latitude"] == 0 | $NOTAM_data["GPS"]["Longitude"] == 0)) {
                    $databaseHandler->addNotam($NOTAM_data["Reference"], $NOTAM_data["Reason"], "", array(array($NOTAM_data["GPS"]["Latitude"], $NOTAM_data["GPS"]["Longitude"])), $NOTAM_data["Radius"]);
                }
            }
        }
    } catch (Exception $e) {
        if (showExceptions)
            print("Exception: " . $e->getMessage());
        else
            die("An error occurred.");
    }
}
