CREATE TRIGGER CreateFirstUserBoard_trg AFTER INSERT ON user
    FOR EACH ROW 
    BEGIN
        DECLARE _userId INT UNSIGNED DEFAULT NULL;
        SELECT MAX(userId)
        INTO _userId 
        FROM user 
        WHERE lastBoardId = NULL;
        INSERT INTO board(userId, boardName, boardDescription) 
                   VALUES(_userId, 'Default', 'Your first board');
    END;