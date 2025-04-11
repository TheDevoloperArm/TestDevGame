<?php
session_start();

require 'db_connect.php';

header('Content-Type: application/json; charset=utf-8');
$user_id = $_POST['id'] ?? '';
$diamond = $_POST['diamond'] ?? '';

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
        $stmt = $conn->prepare("UPDATE user_data SET diamond = :diamond WHERE user_id = :user_id");
        $stmt->execute(['diamond' => $diamond, 'user_id' => $user_id]);

        $stmt = $conn->prepare("SELECT * FROM user_data WHERE user_id = :user_id");
        $stmt->execute(['user_id' => $user_id]);
        $result = $stmt->fetch(PDO::FETCH_ASSOC);

        echo json_encode([
            "status" => "success",
            "message" => "Add diamond success",
            "id" => $result['user_id'],
            "diamond" => $result['diamond'],
            "heart" => $result['heart']
        ]);
    } else {
        echo json_encode(["status" => "error", "message" => "Can't found current username (current user_id is: " . $user_id . ")"]);
    }
} catch (PDOException $e) {
    echo json_encode(["status" => "error", "message" => "An error occurred in the system : " . $e->getMessage()]);
}
