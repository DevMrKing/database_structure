<?php

/**
 * 获得mysql-pdo的连接字符串
 */
function get_mysql_dsn(){
    //数据库类型
    $dbms=trim($_POST['dbms']);
    //数据库主机名
    $host=trim($_POST['host']);
    //使用端口
    $port=trim($_POST['port']);
    //使用的数据库
    $dbname=trim($_POST['dbname']);
    $dsn="$dbms:host=$host;port=$port;dbname=$dbname";
    return $dsn;
}

/**
 * 获取mysql-数据库连接
 */ 
function get_mysql_connect(){
    //初始化一个PDO对象
    $dsn=get_mysql_dsn();
    //数据库连接用户名
    $user=trim($_POST['user']);
    //对应的密码    
    $pass=trim($_POST['pass']);
    //设置编码格式
    $option=array(PDO::MYSQL_ATTR_INIT_COMMAND => "set names utf8");
    $connect = new PDO($dsn,$user,$pass,$option);
    return $connect;
}

/**
 * 测试mysql数据库连接
 * @return true:连接成功，false:连接失败
 */
function test_mysql_connect(){
    //获取连接
    $connect=get_mysql_connect();
    //测试连接
    $select = $connect->query('select count(1)');
    //获取返回值
    $meta = $select->getColumnMeta(0);
    return $meta>0;
}

/**
 * 获得mysql数据库的所有表
 */
function mysql_tables(){
    //获取连接
    $connect=get_mysql_connect();
    //设置模式
    $connect->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    //查询sql
    $sql="SELECT table_name,table_comment FROM information_schema.tables WHERE table_schema=?";
    //预处理sql
    $select=$connect->prepare($sql);
    //查询变量
    $select->execute(array($_POST['dbname']));
    //返回数据样式
    $data = $select->fetchAll(PDO::FETCH_OBJ);
    return $data;
}

/**
 * 获得mysql数据库的所有表结构
 */
function mysql_generation(){
    //获取连接
    $connect=get_mysql_connect();
    //设置模式
    $connect->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    //查询sql
    $sql_tb="SELECT table_name,table_comment FROM information_schema.tables WHERE table_schema=? AND table_name IN (".$_POST['tb'].")";
    //预处理sql
    $select=$connect->prepare($sql_tb);
    //执行查询
    $select->execute(array($_POST['dbname']));
    //返回所有表名有关的数据
    $tabls=$select->fetchAll(PDO::FETCH_OBJ);
    //循环处理所有表名，放入$tables总返回数据
    for ($i=0; $i<count($tabls); $i++) {
        //表的描述
        $sql_col_field='column_name,column_type,column_default,is_nullable,extra,column_key,column_comment ';
        $sql_col="SELECT $sql_col_field FROM information_schema.columns WHERE table_schema=? AND table_name=?";
        //预处理sql
        $select=$connect->prepare($sql_col);
        //执行查询
        $select->execute(array($_POST['dbname'],$tabls[$i]->table_name));
        $tabls[$i]->colums=$select->fetchAll(PDO::FETCH_OBJ);
    }
    return $tabls;
}

/**
 * 另存为html文件
 */
function mysql_save_html(){
    //表结构数据
    $tabls=mysql_generation();
    //数据序列化
    $tableData=json_encode($tabls);
    //文件名
    $dir=dirname(__FILE__);
    $fpath=$dir.'/../template/mysql/mysql_save.html';
    //文件内容
    $handle = fopen($fpath, "r");
    //通过filesize获得文件大小，将整个文件读到一个字符串中
    $file_content = fread($handle, filesize ($fpath));
    fclose($handle);
    //替换内容
    $file_content=str_replace('#tableDataJson#',$tableData,$file_content);
    return $file_content;
}
?>