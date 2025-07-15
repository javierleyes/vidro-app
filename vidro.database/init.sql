-- Create Visit table
CREATE TABLE IF NOT EXISTS visit (
    id SERIAL PRIMARY KEY,
    date TIMESTAMP WITH TIME ZONE NOT NULL,
    address VARCHAR(50) NOT NULL,
    name VARCHAR(20) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    create_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    is_deleted BOOLEAN NOT NULL DEFAULT FALSE
);

-- Create index on Date for better query performance
CREATE INDEX IX_Visit_Date ON visit (Date);
