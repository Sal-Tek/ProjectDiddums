<?php
/*
 * Copyright (c) 2008 Peter Chng, http://unitstep.net/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

/**
 * Decodes a polyline that was encoded using the Google Maps method.
 *
 * The encoding algorithm is detailed here:
 * http://code.google.com/apis/maps/documentation/polylinealgorithm.html
 *
 * This function is based off of Mark McClure's JavaScript polyline decoder
 * (http://facstaff.unca.edu/mcmcclur/GoogleMaps/EncodePolyline/decode.js)
 * which was in turn based off Google's own implementation.
 *
 * This function assumes a validly encoded polyline.  The behaviour of this
 * function is not specified when an invalid expression is supplied.
 *
 * @param String $encoded the encoded polyline.
 * @return Array an Nx2 array with the first element of each entry containing
 *  the latitude and the second containing the longitude of the
 *  corresponding point.
 */
function decodePolylineToArray($encoded)
{
  $length = strlen($encoded);
  $index = 0;
  $points = array();
  $lat = 0;
  $lng = 0;

  while ($index < $length)
  {
    $b = 0;
    $shift = 0;
    $result = 0;
    do
    {
      $b = ord(substr($encoded, $index++)) - 63;
      $result |= ($b & 0x1f) << $shift;
      $shift += 5;
    }
    while ($b >= 0x20);
    $dlat = (($result & 1) ? ~($result >> 1) : ($result >> 1));
    $lat += $dlat;
    $shift = 0;
    $result = 0;
    do
    {
      $b = ord(substr($encoded, $index++)) - 63;
      $result |= ($b & 0x1f) << $shift;
      $shift += 5;
    }
    while ($b >= 0x20);
    $dlng = (($result & 1) ? ~($result >> 1) : ($result >> 1));
    $lng += $dlng;
    $points[] = array($lat * 1e-5, $lng * 1e-5);
  }

  return $points;
}
?>
