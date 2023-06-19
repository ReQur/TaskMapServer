DROP TRIGGER UpdateUserLastBoard_trg;

CREATE TRIGGER UpdateUserLastBoard_trg BEFORE DELETE ON board
    FOR EACH ROW 
    BEGIN
        UPDATE
             user
        SET lastBoardId = ( SELECT MIN(boardId)
                            FROM user_to_board
                            WHERE userId IN(SELECT userId FROM user WHERE lastBoardId = OLD.boardId)
                            AND boardId != OLD.boardId)
        WHERE userId IN(SELECT userId FROM user WHERE lastBoardId = OLD.boardId);
    END;