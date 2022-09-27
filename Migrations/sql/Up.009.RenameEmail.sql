ALTER TABLE user CHANGE email username VARCHAR(255) NOT NULL;

DROP PROCEDURE RegisterUser_proc;

CREATE PROCEDURE RegisterUser_proc(
_username VARCHAR(255),
_firstName VARCHAR(255),
_lastName VARCHAR(255),
_md5PasswordHash VARCHAR(255)
)
BEGIN
    DECLARE _userId INT UNSIGNED DEFAULT NULL;
    
    INSERT INTO user(
        username, 
        firstName, 
        lastName, 
        md5PasswordHash
        ) 
    VALUES(
        _username, 
        _firstName, 
        _lastName, 
        _md5PasswordHash);

    SELECT LAST_INSERT_ID()
    INTO _userId;

    INSERT INTO board(
        username, 
        boardName, 
        boardDescription) 
    VALUES(
        _username, 
        'Default', 
        'Your first board');

    UPDATE user
    SET lastBoardId = (SELECT LAST_INSERT_ID())
    WHERE userId = _userId;
END;