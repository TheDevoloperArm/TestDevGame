<?php
session_start();
require 'db_connect.php';

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $username = trim($_POST['username']);
    $password = $_POST['password'];

    $stmt = $conn->prepare("SELECT * FROM users WHERE username = ?");
    $stmt->execute([$username]);
    $user = $stmt->fetch();
    if ($user && password_verify($password, $user['password'])) {
        $_SESSION['user'] = $user['username'];

        $stmt = $conn->prepare('SELECT id FROM users WHERE username = ?');
        $stmt->execute([$username]);
        $_SESSION['user_id'] = $user['id'];
        
        header("Location: get_data.php");
        exit;
    } else {
        echo "ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
    }
}
?>

<form method="post">
    ชื่อผู้ใช้: <input type="text" name="username"><br>
    รหัสผ่าน: <input type="password" name="password"><br>
    <button type="submit">เข้าสู่ระบบ</button>
</form>