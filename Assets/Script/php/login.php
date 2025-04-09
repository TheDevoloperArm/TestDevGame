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
            echo json_encode([
                "status" => "success",
                "user" => $result['username'],
                "diamond" => $result['diamond'],
                "heart" => $result['heart']
            ]);
        } else {
            echo json_encode(["status" => "error", "message" => "รหัสผ่านไม่ถูกต้อง"]);
        }
    } else {
        echo json_encode(["status" => "error", "message" => "ไม่พบชื่อผู้ใช้งานนี้"]);
    }
} catch (PDOException $e) {
    echo json_encode(["status" => "error", "message" => "เกิดข้อผิดพลาดในระบบฐานข้อมูล : " . $e->getMessage()]);
}

// if ($_SERVER['REQUEST_METHOD'] == 'POST') {
//     $username = trim($_POST['username']);
//     $password = $_POST['password'];

//     $stmt = $conn->prepare("SELECT * FROM users WHERE username = ?");
//     $stmt->execute([$username]);
//     $user = $stmt->fetch();
//     if ($user && password_verify($password, $user['password'])) {
//         $_SESSION['user'] = $user['username'];

//         $stmt = $conn->prepare('SELECT id FROM users WHERE username = ?');
//         $stmt->execute([$username]);
//         $_SESSION['user_id'] = $user['id'];

//         header("Location: get_data.php");
//         exit;
//     } else {
//         echo "ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
//     }
// }