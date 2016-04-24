-- phpMyAdmin SQL Dump
-- version 4.6.0
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Apr 24, 2016 at 10:33 AM
-- Server version: 5.7.11-0ubuntu6
-- PHP Version: 7.0.4-7ubuntu2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `no-fly`
--

-- --------------------------------------------------------

--
-- Table structure for table `airspace`
--

CREATE TABLE `airspace` (
  `airCategory` char(1) NOT NULL,
  `airName` varchar(40) NOT NULL,
  `airType` varchar(40) NOT NULL,
  `flLower` int(3) NOT NULL,
  `flUpper` int(3) NOT NULL,
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `airspace_polypoints`
--

CREATE TABLE `airspace_polypoints` (
  `lat` decimal(9,6) NOT NULL,
  `lon` decimal(9,6) NOT NULL,
  `refId` int(11) NOT NULL,
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `notam`
--

CREATE TABLE `notam` (
  `ntReference` varchar(10) NOT NULL,
  `ntMeaning` text NOT NULL,
  `ntSuffix` text NOT NULL,
  `lat` decimal(9,6) DEFAULT NULL,
  `lon` decimal(9,6) DEFAULT NULL,
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `notam_polypoints`
--

CREATE TABLE `notam_polypoints` (
  `lat` decimal(9,6) NOT NULL,
  `lon` decimal(9,6) NOT NULL,
  `rad` int(1) DEFAULT NULL,
  `refId` int(11) NOT NULL,
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `airspace`
--
ALTER TABLE `airspace`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `airspace_polypoints`
--
ALTER TABLE `airspace_polypoints`
  ADD PRIMARY KEY (`id`),
  ADD KEY `refId` (`refId`);

--
-- Indexes for table `notam`
--
ALTER TABLE `notam`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `notam_polypoints`
--
ALTER TABLE `notam_polypoints`
  ADD PRIMARY KEY (`id`),
  ADD KEY `refId` (`refId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `airspace`
--
ALTER TABLE `airspace`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `airspace_polypoints`
--
ALTER TABLE `airspace_polypoints`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `notam`
--
ALTER TABLE `notam`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `notam_polypoints`
--
ALTER TABLE `notam_polypoints`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
