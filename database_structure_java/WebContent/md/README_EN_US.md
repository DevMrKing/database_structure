# Database document generation tool：
## src
#### db：Database operation class
#### service：Database service class
#### servlet：Background processing total entrance
#### util：Tools
#### vo：Encapsulated entity class

## WebContent
#### i18n：International language pack
#### md：Translation instructions for different languages
#### template：Template processing files written for different databases
#### view：View processing
#### WEB-INF/lib：Third-party supported jar

#### common.js Common js processing throughout the project
#### common.css Common css processing throughout the project
#### index.html Entrance
#### jquery-1.11.0.min.js jquery
#### template-web-4.13.2.js Template rendering engine

#### Third-party support needed：fastjson，oracle-jdbc，mysql-jdbc，sqlserver-jdbc,postgresql-jdbc

#### The author wrote this project using jdk7

# Secondary development：
#### Add new language support：
1. Shortcode for browser support for language in the i18ns array in common.js
2. Support for language options in the lang_arr array in common.js
3. Append language pack js under i18n folder

#### Add new database support：
1. In the common.js support_database array plus the database to be supported
2. In the db, service package to add support for the database processing java file
3. In servlet/FunctionServlet.java plus if processing to support the database
4. In common.js, table_html(), generation_html(), plus the special template path to be jumped.
5. In the new template, the template should be added with custom processing code.
