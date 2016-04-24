<?php
require_once("database.php");
$debug = true;
// Gather our GET vars
$latitude = (is_numeric($_GET['lat'])) ? $_GET['lat'] : false;
$longitude = (is_numeric($_GET['lng'])) ? $_GET['lng'] : false;
$radius = (is_numeric($_GET['rad'])) ? $_GET['rad'] : false;
// Primitive validation
if (!($latitude && $longitude && $radius)) die("Invalid parameters.");
// Let's get that juicy database
$databaseHandler = new databaseHandler();
$databaseHandler->db_connect();

// GATHER DISTINCT POINTS NEARBY
// NOTAM
try {
    $gatherNotam = $databaseHandler->database->prepare("SELECT
	3956 * 2 * ASIN(
	SQRT(
		POWER(
			SIN(
				(notam_polypoints.lat - abs(:latDec)) * pi() / 180 / 2
			),
			2
		) + COS(notam_polypoints.lat * pi() / 180) * COS(abs(:latDec) * pi() / 180) * POWER(
			SIN(
				(notam_polypoints.lon - :lngDec) * pi() / 180 / 2
			),
			2
		)
	)
	) AS distance,
	notam.*
FROM
	notam_polypoints
LEFT JOIN 
	notam ON notam.id = notam_polypoints.refId
GROUP BY
	notam.id
HAVING
	distance < :radius
ORDER BY
	distance");
    $gatherNotam->execute(array(":latDec" => $latitude, ":lngDec" => $longitude, ":radius" => $radius));
    $nearbyNotam = $gatherNotam->fetchAll(PDO::FETCH_ASSOC);
    //die($gatherNotam->rowCount());
    foreach ($nearbyNotam as $key => $value) {
        // Inject polypoints
        $gatherPolypoints = $databaseHandler->database->prepare("SELECT lat, lon, rad FROM notam_polypoints WHERE refId = ?");
        $gatherPolypoints->execute(array($value["id"]));
        $nearbyNotam[$key]["polypoints"] = $gatherPolypoints->fetchAll(PDO::FETCH_ASSOC);
    }
    print(json_encode($nearbyNotam));
} catch (PDOException $e) {
    if($debug) die($e->getMessage());
    else die("Something went wrong while gathering NOTAMs.");
}

// Airspace
try {
    $gatherAirspace = $databaseHandler->database->prepare("SELECT
	3956 * 2 * ASIN(
	SQRT(
		POWER(
			SIN(
				(lat - abs(:latDec)) * pi() / 180 / 2
			),
			2
		) + COS(lat * pi() / 180) * COS(abs(:latDec) * pi() / 180) * POWER(
			SIN(
				(lon - :lngDec) * pi() / 180 / 2
			),
			2
		)
	)
	) AS distance,
	airspace.*
FROM
	airspace_polypoints
LEFT JOIN 
	airspace ON airspace.id = airspace_polypoints.refId
GROUP BY
	airspace.id
HAVING
	distance < :radius
ORDER BY
	distance");
    $gatherAirspace->execute(array(":latDec" => $latitude, ":lngDec" => $longitude, ":radius" => $radius));
    $nearbyAirspace = $gatherAirspace->fetchAll(PDO::FETCH_ASSOC);
    foreach ($nearbyAirspace as $key => $value) {
        // Inject polypoints
        $gatherPolypoints = $databaseHandler->database->prepare("SELECT lat, lon FROM airspace_polypoints WHERE refId = ?");
        $gatherPolypoints->execute(array($value["id"]));
        $nearbyAirspace[$key]["polypoints"] = $gatherPolypoints->fetchAll(PDO::FETCH_ASSOC);
    }
    print(json_encode($nearbyAirspace));
} catch (PDOException $e) {
    if($debug) die($e->getMessage());
    else die("Something went wrong while gathering NOTAMs.");
}
$databaseHandler->db_disconnect();
