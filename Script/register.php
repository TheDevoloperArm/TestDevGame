<?php
session_start();
require 'db_connect.php';

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $username = trim($_POST['username']);
    $password = $_POST['password'];

    if ($username && $password) {
        $stmt = $conn->prepare("SELECT id FROM users WHERE username = ?");
        $stmt->execute([$username]);
        if ($stmt->fetch()) {
            echo "ชื่อผู้ใช้นี้ถูกใช้แล้ว";
        } else {
            $hashed = password_hash($password, PASSWORD_DEFAULT);
            $stmt = $conn->prepare("INSERT INTO users (username, password) VALUES (?, ?)");
            if ($stmt->execute([$username, $hashed])) {
                echo "สมัครสมาชิกสำเร็จ <a href='login.php'>เข้าสู่ระบบ</a>";
                $user_id = $conn->lastInsertId();
                $stmt = $conn->prepare("INSERT INTO user_data (user_id) VALUES (?)");
                $stmt->execute([$user_id]);
            } else {
                echo "เกิดข้อผิดพลาด";
            }
        }
    } else {
        echo "กรุณากรอกข้อมูลให้ครบถ้วน";
    }
}
?>

<form method="post">
    ชื่อผู้ใช้: <input type="text" name="username"><br>
    รหัสผ่าน: <input type="password" name="password"><br>
    <button type="submit">สมัครสมาชิก</button>
</form>