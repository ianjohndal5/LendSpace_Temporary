-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 16, 2025 at 02:05 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `lendspace`
--

-- --------------------------------------------------------

--
-- Table structure for table `announcements`
--

CREATE TABLE `announcements` (
  `id` int(11) NOT NULL,
  `title` varchar(255) NOT NULL,
  `content` text NOT NULL,
  `publish_date` datetime NOT NULL,
  `expiry_date` datetime DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1,
  `priority` varchar(20) NOT NULL DEFAULT 'Medium'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `bookings`
--

CREATE TABLE `bookings` (
  `id` int(11) NOT NULL,
  `facility_id` int(11) NOT NULL,
  `user_email` varchar(100) NOT NULL,
  `user_name` varchar(100) NOT NULL,
  `booking_date` date NOT NULL,
  `start_time` time NOT NULL,
  `end_time` time NOT NULL,
  `attendee_count` int(11) NOT NULL,
  `purpose` text NOT NULL,
  `total_cost` decimal(10,2) NOT NULL,
  `created_at` datetime NOT NULL,
  `status` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bookings`
--

INSERT INTO `bookings` (`id`, `facility_id`, `user_email`, `user_name`, `booking_date`, `start_time`, `end_time`, `attendee_count`, `purpose`, `total_cost`, `created_at`, `status`) VALUES
(1, 1, 'ianjohndal5@gmail.com', 'IanJohn', '2025-04-16', '09:00:00', '17:00:00', 1, 'Vacation', 400.00, '2025-04-16 17:40:18', 'Cancelled');

-- --------------------------------------------------------

--
-- Table structure for table `facilities`
--

CREATE TABLE `facilities` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` text NOT NULL,
  `location` varchar(100) NOT NULL,
  `capacity` int(11) NOT NULL,
  `hourly_rate` decimal(10,2) NOT NULL,
  `image_url` varchar(255) NOT NULL,
  `is_available` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `facilities`
--

INSERT INTO `facilities` (`id`, `name`, `description`, `location`, `capacity`, `hourly_rate`, `image_url`, `is_available`) VALUES
(1, 'Conference Room A', 'A spacious conference room with a large table, comfortable chairs, and state-of-the-art presentation equipment. Perfect for business meetings, workshops, and seminars.', 'Building 1, Floor 2', 20, 50.00, '/images/conference-room-a.jpg', 1),
(2, 'Executive Boardroom', 'An elegant boardroom with executive chairs, a mahogany table, and high-end video conferencing capabilities. Ideal for important board meetings and client presentations.', 'Building 1, Floor 3', 12, 75.00, '/images/executive-boardroom.jpg', 1),
(3, 'Training Room', 'A versatile training room with flexible seating arrangements, multiple screens, and interactive whiteboards. Great for training sessions, classes, and group exercises.', 'Building 2, Floor 1', 30, 60.00, '/images/training-room.jpg', 1),
(4, 'Creative Space', 'An inspirational space with movable furniture, whiteboards on all walls, and colorful design. Perfect for brainstorming sessions and creative team activities.', 'Building 2, Floor 2', 15, 45.00, '/images/creative-space.jpg', 1),
(5, 'Lecture Hall', 'A tiered lecture hall with comfortable seating, excellent acoustics, and a large projection screen. Ideal for presentations, lectures, and panel discussions.', 'Building 3, Floor 1', 50, 80.00, '/images/lecture-hall.jpg', 1);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `name` varchar(100) NOT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `department` varchar(100) DEFAULT NULL,
  `is_admin` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `announcements`
--
ALTER TABLE `announcements`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `bookings`
--
ALTER TABLE `bookings`
  ADD PRIMARY KEY (`id`),
  ADD KEY `facility_id` (`facility_id`);

--
-- Indexes for table `facilities`
--
ALTER TABLE `facilities`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `announcements`
--
ALTER TABLE `announcements`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `bookings`
--
ALTER TABLE `bookings`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `facilities`
--
ALTER TABLE `facilities`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bookings`
--
ALTER TABLE `bookings`
  ADD CONSTRAINT `bookings_ibfk_1` FOREIGN KEY (`facility_id`) REFERENCES `facilities` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
