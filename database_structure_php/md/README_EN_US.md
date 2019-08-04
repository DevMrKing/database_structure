# Database document generation tool：
## i18n：International language pack
## md：Translation instructions for different languages
## php：php Background processing
## template：Template processing files written for different databases
## view：View processing

## common.js Common js processing throughout the project
## common.css Common css processing throughout the project
## index.html Entrance
## jquery-1.11.0.min.js jquery
## template-web-4.13.2.js Template rendering engine

#### Third-party extensions that need to be used：json,pdo_mysql,pdo_pgsql,pdo_sqlsrv,(instantclient_12_1,php_pdo_oci,php_oci8,php_oci8_12c)

#### The author wrote this project using php7.2

# Secondary development：
#### Add new language support：
1. Shortcode for browser support for language in the i18ns array in common.js
2. Support for language options in the lang_arr array in common.js
3. Append language pack js under i18n folder

#### Add new database support：
1. In the common.js support_database array plus the database to be supported
2. Append the support database to the php file under the php folder
3. In the fun.php plus for if and require_once processing to support the database
4. In common.js, table_html(), generation_html(), plus the special template path to be jumped.
5. In the new template, the template should be added with custom processing code.
