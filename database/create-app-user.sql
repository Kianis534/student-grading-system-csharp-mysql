-- =============================================================================
-- Optional: create a least-privilege MySQL account for the application
--
-- Running the app as MySQL root works, but a dedicated account that can only
-- read and write the StudentGradingSystem tables is safer.
--
-- 1. Replace CHANGE_ME_TO_A_STRONG_PASSWORD below with your own password.
-- 2. Run:  mysql -u root -p < database/create-app-user.sql
-- 3. Use  User ID=sgs_app;Password=<your password>  in Database.config.
--
-- Do not commit this file with a real password in it.
-- =============================================================================

CREATE USER IF NOT EXISTS 'sgs_app'@'localhost'
    IDENTIFIED BY 'CHANGE_ME_TO_A_STRONG_PASSWORD';

GRANT SELECT, INSERT, UPDATE, DELETE
    ON StudentGradingSystem.*
    TO 'sgs_app'@'localhost';

FLUSH PRIVILEGES;
