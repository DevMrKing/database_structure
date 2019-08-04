var lang={
    'language':'Language',
    'title':'Database Reverse Generation Table Structure Document Tool',
    'steps':['Language','Connect','Table','Generation'],
    'connect_form':{
        'type_lbl':'dialect',
        'host_lbl':'host',
        'port_lbl':'port',
        'db_lbl':'db name',
        'uname_lbl':'user',
        'pwd_lbl':'password',
        'test_lbl':'test',
        'con_lbl':'connect'
    },
    'connect_form_validate':{
        'host_required_lbl':'Please enter the host!',
        'port_required_lbl':'Please enter the port!',
        'db_required_lbl':'Please enter the DBName!',
        'uname_required_lbl':'Please enter the user!',
        'pwd_required_lbl':'Please enter the password!',
        'connect_success':'SUCCESS'
    },
    'tables_form':{
        'pvcols':['table name','explain'],
        'create_btn':'generate',
        'table_required':'Please select a table!',
        'search_input_placeholder':'table name'
    },
    'generation_form':{
        'pvcols':['field','data_type','default','is_null','auto_increment','primary_key','comment'],
        'save_as':'save As Html',
        'search_input_placeholder':'table name'
    }
};