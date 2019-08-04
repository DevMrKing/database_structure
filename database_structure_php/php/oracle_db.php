<?php

/**
 * 获取oracle-数据库连接
 */
function get_oracle_connect()
{
    //数据库主机名
    $host = 'mysql';
    //使用端口
    $port = trim($_POST['port']);
    //使用的数据库
    $dbname = trim($_POST['dbname']);
    //初始化一个PDO对象
    $dsn = "$host:$port/$dbname";
    //数据库连接用户名
    $user = trim($_POST['user']);
    //对应的密码
    $pass = trim($_POST['pass']);
    //设置编码格式
    $connect = oci_connect($user, $pass, $dsn, 'UTF8');
    return $connect;
}

/**
 * 测试oracle数据库连接
 * @return true:连接成功，false:连接失败
 */
function test_oracle_connect()
{
    //获取连接
    $connect = get_oracle_connect();
    //测试连接
    $statement = oci_parse($connect, 'select count(1) AS "count" from dual');
    //执行sql
    oci_execute($statement);
    //获取返回值
    $ret = oci_fetch_object($statement);
    //关闭连接
    oci_free_statement($statement);
    oci_close($connect);
    return $ret->count > 0;
}

/**
 * 获得oracle数据库的所有表
 */
function oracle_tables()
{
    //获取连接
    $connect = get_oracle_connect();
    //查询sql
    $sql = 'SELECT t.table_name AS "table_name",f.comments AS "table_comment" FROM user_tables t 
    INNER JOIN user_tab_comments f ON t.table_name = f.table_name';
    //解析sql
    $statement = oci_parse($connect, $sql);
    //执行sql
    oci_execute($statement);
    //获取返回值
    $results = array();
    while (($row = oci_fetch_array($statement,OCI_ASSOC)) != false) {
        array_push($results,$row);
    }
    //关闭连接
    oci_free_statement($statement);
    oci_close($connect);
    return $results;
}

/**
 * 获得oracle数据库的所有表结构
 */
function oracle_generation()
{
    //获取连接
    $connect = get_oracle_connect();
    //查询sql
    $sql_tb = 'SELECT t.table_name AS "table_name",f.comments AS "table_comment" FROM user_tables t 
    INNER JOIN user_tab_comments f ON t.table_name = f.table_name
    WHERE t.table_name IN ('.$_POST['tb'].')';
    //解析sql
    $tbstatement = oci_parse($connect, $sql_tb);
    //执行sql
    $results = array();
    oci_execute($tbstatement);
    while (($tbrow = oci_fetch_array($tbstatement,OCI_ASSOC)) != false) {
        array_push($results,$tbrow);
    }
    //释放重新获取连接
    oci_free_statement($tbstatement);
    for ($i=0; $i<count($results); $i++) {
        $table_name=$results[$i]['table_name'];
        //查询列数据
        $sql_col='SELECT 
            col.column_name AS "column_name",
            col.data_type AS "column_type",
            col.data_default AS "column_default",
            col.nullable AS "is_nullable",
            cns.constraint_type AS "column_key",
            ucc.comments AS "column_comment"
            FROM user_tab_columns col
            LEFT JOIN user_col_comments ucc ON ucc.table_name=col.table_name AND ucc.column_name=col.column_name
            LEFT JOIN user_cons_columns ccs ON ccs.table_name=col.table_name AND ccs.column_name=col.column_name AND ccs.position=col.column_id
            LEFT JOIN user_constraints cns ON col.table_name=cns.table_name AND cns.constraint_type=\'P\' AND ccs.constraint_name=cns.constraint_name
            WHERE col.table_name =:table_name
            ORDER BY col.column_id ASC
            ';
        $colstatement = oci_parse($connect, $sql_col);
        oci_bind_by_name($colstatement, ":table_name", $table_name);
        oci_execute($colstatement);
        $colums = array();
        while (($cols = oci_fetch_array($colstatement,OCI_ASSOC)) != false) {
            array_push($colums,$cols);
        }
        $results[$i]['colums']=$colums;
    }
    oci_close($connect);
    return $results;
}

/**
 * 另存为html文件
 */
function oracle_save_html()
{
    //表结构数据
    $tabls = oracle_generation();
    //数据序列化
    $tableData = json_encode($tabls);
    //文件名
    $dir = dirname(__FILE__);
    $fpath = $dir . '/../template/postgresql/postgresql_save.html';
    //文件内容
    $handle = fopen($fpath, "r");
    //通过filesize获得文件大小，将整个文件读到一个字符串中
    $file_content = fread($handle, filesize($fpath));
    fclose($handle);
    //替换内容
    $file_content = str_replace('#tableDataJson#', $tableData, $file_content);
    return $file_content;
}

?>