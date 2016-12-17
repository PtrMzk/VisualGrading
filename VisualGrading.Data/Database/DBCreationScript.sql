PRAGMA foreign_keys = "0";
PRAGMA encoding
SELECT type,name,sql,tbl_name FROM sqlite_master UNION SELECT type,name,sql,tbl_name FROM sqlite_temp_master;
CREATE TABLE `Grade` (
	`GradeID`	TEXT NOT NULL,
	`Points`	NUMERIC DEFAULT 0,
	`TestID`	TEXT,
	`StudentID`	TEXT,
	PRIMARY KEY(`GradeID`)
);
SELECT type,name,sql,tbl_name FROM sqlite_master UNION SELECT type,name,sql,tbl_name FROM sqlite_temp_master;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC LIMIT 0, 50000;
PRAGMA auto_vacuum
PRAGMA automatic_index
PRAGMA checkpoint_fullfsync
PRAGMA foreign_keys
PRAGMA fullfsync
PRAGMA ignore_check_constraints
PRAGMA journal_mode
PRAGMA journal_size_limit
PRAGMA locking_mode
PRAGMA max_page_count
PRAGMA page_size
PRAGMA recursive_triggers
PRAGMA secure_delete
PRAGMA synchronous
PRAGMA temp_store
PRAGMA user_version
PRAGMA wal_autocheckpoint
SELECT type,name,sql,tbl_name FROM sqlite_master UNION SELECT type,name,sql,tbl_name FROM sqlite_temp_master;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC LIMIT 0, 50000;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` DESC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` DESC LIMIT 0, 50000;
SELECT type,name,sql,tbl_name FROM sqlite_master UNION SELECT type,name,sql,tbl_name FROM sqlite_temp_master;
CREATE TABLE `Test` (
	`TestID`	TEXT NOT NULL,
	`Name`	TEXT,
	`Subject`	TEXT,
	`SubCategory`	TEXT,
	`Date`	TEXT,
	`SeriesNumber`	INTEGER,
	`MaximumPoints`	INTEGER,
	PRIMARY KEY(`TestID`)
);
SELECT type,name,sql,tbl_name FROM sqlite_master UNION SELECT type,name,sql,tbl_name FROM sqlite_temp_master;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC LIMIT 0, 50000;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` DESC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` DESC LIMIT 0, 50000;
CREATE TABLE `Student` (
	`StudentID`	TEXT NOT NULL,
	`FirstName`	TEXT,
	`LastName`	TEXT,
	`Nickname`	TEXT,
	`EmailAddress`	TEXT,
	`ParentEmailAddress`	TEXT,
	PRIMARY KEY(`StudentID`)
);
SELECT type,name,sql,tbl_name FROM sqlite_master UNION SELECT type,name,sql,tbl_name FROM sqlite_temp_master;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC LIMIT 0, 50000;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` DESC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` DESC LIMIT 0, 50000;
CREATE TABLE `sqlitebrowser_rename_column_new_table` (
	`GradeID`	TEXT NOT NULL,
	`Points`	NUMERIC DEFAULT 0,
	`TestID`	TEXT,
	`StudentID`	TEXT,
	PRIMARY KEY(`GradeID`),
	FOREIGN KEY(`TestID`) REFERENCES Test.TestID
);
CREATE TABLE `sqlitebrowser_rename_column_new_table` (
	`GradeID`	TEXT NOT NULL,
	`Points`	NUMERIC DEFAULT 0,
	`TestID`	TEXT,
	`StudentID`	TEXT,
	PRIMARY KEY(`GradeID`),
	FOREIGN KEY(`StudentID`) REFERENCES Student.StudentID
);
SELECT type,name,sql,tbl_name FROM sqlite_master UNION SELECT type,name,sql,tbl_name FROM sqlite_temp_master;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` ASC LIMIT 0, 50000;
SELECT COUNT(*) FROM (SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` DESC);
SELECT `_rowid_`,* FROM `Grade` ORDER BY `_rowid_` DESC LIMIT 0, 50000;
SELECT type,name,sql,tbl_name FROM sqlite_master UNION SELECT type,name,sql,tbl_name FROM sqlite_temp_master;
