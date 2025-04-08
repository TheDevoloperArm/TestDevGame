<?php
session_start();
if (!isset($_SESSION["user"])) {
    header("Location: login.php");
    exit();
}
require 'db_connect.php'; // เรียกไฟล์เชื่อมต่อ

header('Content-Type: application/json; charset=utf-8');

$stmt = $conn->query("SELECT id, username FROM users");
$data = $stmt->fetchAll(PDO::FETCH_ASSOC);

echo json_encode(["user" => $data]);