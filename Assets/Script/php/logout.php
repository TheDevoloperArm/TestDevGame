<?php
session_start();
require 'db_connect.php';
header('Content-Type: application/json; charset=utf-8');
$user_id = $_POST['id'] ?? '';
$diamond = $_POST['diamond'] ?? '';
$heart = $_POST['heart'] ?? '';

if (empty($user_id)) {
    echo json_encode(["status" => "Missing user_id"]);
    echo "error Missing user_id";
    exit;
}

try {
    $stmt = $conn->prepare("SELECT user_id FROM user_data WHERE user_id = :user_id");
    $stmt->execute(['user_id' => $user_id]);
    $result = $stmt->fetch(PDO::FETCH_ASSOC);

    if ($result) {
        $stmt = $conn->prepare("UPDATE user_data SET diamond = :diamond, heart = :heart WHERE user_id = :user_id");
        $stmt->execute(['diamond' => $diamond, 'heart' => $heart, 'user_id' => $user_id]);

        $stmt = $conn->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
        $stmt->execute(['user_id' => $user_id]);
        $result = $stmt->fetch(PDO::FETCH_ASSOC);

        echo json_encode(["status" => "success"]);

        session_destroy();
        exit;
    } else {
        echo json_encode(["status" => "error", "message" => "Can't logout"]);
    }
} catch (PDOException $e) {
    echo json_encode(["status" => "error", "message" => "An error occurred in the system : " . $e->getMessage()]);
}
