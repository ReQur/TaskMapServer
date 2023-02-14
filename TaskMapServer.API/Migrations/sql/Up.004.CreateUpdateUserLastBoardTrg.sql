CREATE TRIGGER UpdateUserLastBoard_trg BEFORE DELETE ON board
    FOR EACH ROW 
    BEGIN
        DECLARE _userId INT UNSIGNED DEFAULT NULL;
        SELECT userId 
        INTO _userId 
        FROM user 
        WHERE lastBoardId = OLD.boardId;

        IF _userId
        THEN UPDATE user 
             SET lastBoardId = (SELECT MIN(boardId)
                                FROM board
                                WHERE userId = _userId AND boardId != OLD.boardId) 
             WHERE userId = _userId;
        END IF;
    END;