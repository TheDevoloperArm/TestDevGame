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

    if ($stmt->fetch(PDO::FETCH_ASSOC)) {
        echo json_encode(["status" => "Error", "message" => "Username is already in use."]);
    } else {
        $hashed = password_hash($password, PASSWORD_DEFAULT);
        $stmt = $conn->prepare("INSERT INTO users (username, password) VALUES (:username, :password)");
        if ($stmt->execute(['username' => $username, 'password' => $hashed])) {
            echo json_encode(["status" => "success", "message" => "Sign up successfully."]);
            $user_id = $conn->lastInsertId();
            $stmt = $conn->prepare("INSERT INTO user_data (user_id) VALUES (:user_id)");
            $stmt->execute(['user_id' => $user_id]);
        } else {
            echo json_encode(["status" => "error", "message" => "Can't sign up current username."]);
        }
    }
} catch (PDOException $e) {
    echo json_encode(["status" => "error", "message" => "An error occurred in the system."]);
}
