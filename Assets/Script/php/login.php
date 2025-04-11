<?php
session_start();
require 'db_connect.php';

header('Content-Type: application/json');

$username = trim($_POST['username']) ?? '';
$password = $_POST['password'] ?? '';

try {
    $stmt = $conn->prepare("SELECT users.id, users.username, users.password, user_data.diamond, user_data.heart FROM users INNER JOIN user_data ON users.id=user_data.user_id WHERE users.username = :username;");
    $stmt->execute(['username' => $username]);
    $result = $stmt->fetch(PDO::FETCH_ASSOC);

    if ($result) {
        if (password_verify($password, $result['password'])) {
            $stmt = $conn->prepare("SELECT user_id FROM login_history WHERE user_id = :user_id");
            $stmt->execute(['user_id' => $result['id']]);
            if ($stmt->fetch()) {
                $stmt = $conn->prepare("UPDATE login_history SET login_time = NOW() WHERE user_id = :user_id");
                $stmt->execute(['user_id' => $result['id']]);
                echo json_encode([
                    "status" => "success",
                    "id" => $result['id'],
                    "user" => $result['username'],
                    "diamond" => $result['diamond'],
                    "heart" => $result['heart']
                ]);
            } else {
                $stmt = $conn->prepare("INSERT INTO login_history (user_id) VALUES (:user_id)");
                $stmt->execute(['user_id' => $result['id']]);
                echo json_encode([
                    "status" => "success",
                    "id" => $result['id'],
                    "user" => $result['username'],
                    "diamond" => $result['diamond'],
                    "heart" => $result['heart']
                ]);
            }
        } else {
            echo json_encode(["status" => "error", "message" => "Wrong password."]);
        }
    } else {
        echo json_encode(["status" => "error", "message" => "Can't found current username."]);
    }
} catch (PDOException $e) {
    echo json_encode(["status" => "error", "message" => "An error occurred in the system : " . $e->getMessage()]);
}