SET GLOBAL log_bin_trust_function_creators = 1;
CREATE PROCEDURE DeleteUserBoard(
    IN  _boardId INT UNSIGNED
)
BEGIN

    DECLARE _userId INT UNSIGNED DEFAULT NULL;
    SELECT userId 
    INTO _userId 
    FROM user 
    WHERE lastBoardId = _boardId;
    IF _userId
    THEN UPDATE user 
         SET lastBoardId = (SELECT MIN(boardId)
                            FROM board
                            WHERE userId = _userId AND boardId != _boardId) 
         WHERE userId = _userId;
    END IF;
    
    DELETE FROM board 
    WHERE boardId = _boardId;
END;