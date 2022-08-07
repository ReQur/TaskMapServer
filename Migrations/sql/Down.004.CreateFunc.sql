DELIMITER //
CREATE FUNCTION defoult_user_board_func(_user UNSIGNED INT)
RETURNS INT

BEGIN
	DECLARE _board INT;
	
	SET _board = MIN(SELECT boardId FROM board WHERE userId = _user);

	RETURN _board;
END; //

DELIMITER ;



