<?php
session_start();
if (!isset($_SESSION["user"])) {
    header("Location: login.php");
    exit();
}
require 'db_connect.php'; // เรียกไฟล์เชื่อมต่อ

header('Content-Type: application/json; charset=utf-8');
$user_id = $_SESSION['user_id'] ?? null;

if (empty($user_id)) {
    echo json_encode(["error" => "Missing user_id"]);
    echo "error Missing user_id";
    exit;
}

$stmt = $conn->prepare("SELECT id, username FROM users WHERE id = ?");
$stmt->execute([$user_id]);

if ($stmt->fetch()) {
    $stmt = $conn->prepare("UPDATE login_history SET login_time = NOW() WHERE user_id = ?");
    $stmt->execute([$user_id]);
} else {
    $stmt = $conn->prepare("INSERT INTO login_history (user_id) VALUES (?)");
    $stmt->execute([$user_id]);
}

// ดึงข้อมูล user_data
$stmt = $conn->prepare("SELECT user_id, diamond, heart FROM user_data WHERE user_id = ?");
$stmt->execute([$user_id]);
$data = $stmt->fetch();
echo json_encode(["user_id" => $data['user_id'], "diamond" => $data['diamond'], "heart" => $data['heart']]);