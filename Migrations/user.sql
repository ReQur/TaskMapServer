CREATE TABLE IF NOT EXISTS `user` (
    `userId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
    `email` VARCHAR(255) NOT NULL,
    `firstName` VARCHAR(255) NOT NULL,
    `lastName` VARCHAR(255) NOT NULL,
    `md5PasswordHash` VARCHAR(255) NOT NULL,
    `lastBoardId` INT(10) UNSIGNED,
    PRIMARY KEY (`userId`),
    UNIQUE KEY (`email`)
);

INSERT INTO user(email, firstName, lastName, md5PasswordHash) VALUES('test@mail.ru', 'testName', 'testLName', '615615vsdvds');