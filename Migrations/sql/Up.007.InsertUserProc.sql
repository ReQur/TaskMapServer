DROP TRIGGER CreateFirstUserBoard_trg;

CREATE PROCEDURE RegisterUser_proc(
_email VARCHAR(255),
_firstName VARCHAR(255),
_lastName VARCHAR(255),
_md5PasswordHash VARCHAR(255)
)
BEGIN
    DECLARE _userId INT UNSIGNED DEFAULT NULL;
    
    INSERT INTO user(
        email, 
        firstName, 
        lastName, 
        md5PasswordHash
        ) 
    VALUES(
        _email, 
        _firstName, 
        _lastName, 
        _md5PasswordHash);

    SELECT LAST_INSERT_ID()
    INTO _userId;

    INSERT INTO board(
        userId, 
        boardName, 
        boardDescription) 
    VALUES(
        _userId, 
        'Default', 
        'Your first board');

    UPDATE user
    SET lastBoardId = (SELECT LAST_INSERT_ID())
    WHERE userId = _userId;
END;