-- ============================================================
-- Database Schema for Premium Calculation & Dynamic Ingestion
-- Target Engine: SQLite
-- ============================================================

-- 1. טבלת שיטות פרמיה (Metadata Layer)
CREATE TABLE IF NOT EXISTS PremiumMethods (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MethodNumber TEXT NOT NULL UNIQUE,
    Description TEXT,
    PremiumPercentage DECIMAL(5,2) NOT NULL,
    CalculationPeriod TEXT NOT NULL
);

-- 2. טבלת מדדים (Metadata Layer)
CREATE TABLE IF NOT EXISTS Measures (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PremiumMethodId INTEGER NOT NULL,
    Name TEXT NOT NULL,
    Description TEXT,
    SourceType TEXT NOT NULL,
    SourceName TEXT NOT NULL,
    Frequency TEXT NOT NULL,
    FOREIGN KEY (PremiumMethodId) REFERENCES PremiumMethods(Id) ON DELETE CASCADE
);

-- 3. טבלת היסטוריית קליטות קבצים (Metadata Layer)
CREATE TABLE IF NOT EXISTS IngestionHistories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MeasureId INTEGER NOT NULL,
    Year INTEGER NOT NULL,
    Period TEXT NOT NULL,
    FileName TEXT NOT NULL,
    IngestedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    RecordCount INTEGER NOT NULL,
    FOREIGN KEY (MeasureId) REFERENCES Measures(Id) ON DELETE CASCADE
);

-- 4. טבלת שמירת נתוני הקובץ הדינמיים (Dynamic Data Layer)
CREATE TABLE IF NOT EXISTS IngestedDataRows (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IngestionHistoryId INTEGER NOT NULL,
    DataJson TEXT NOT NULL,
    FOREIGN KEY (IngestionHistoryId) REFERENCES IngestionHistories(Id) ON DELETE CASCADE
);

-- ============================================================
-- Indexes for Performance Optimization
-- ============================================================

-- אינדקס לשליפת מדדים לפי שיטת פרמיה
CREATE INDEX IF NOT EXISTS IX_Measures_PremiumMethodId 
    ON Measures(PremiumMethodId);

-- אינדקס לשליפת היסטוריית קליטות לפי מדד ותאריך
CREATE INDEX IF NOT EXISTS IX_IngestionHistories_MeasureId_IngestedAt 
    ON IngestionHistories(MeasureId, IngestedAt DESC);

-- אינדקס קריטי לשליפת שורות ה-JSON עבור קליטה מסוימת
CREATE INDEX IF NOT EXISTS IX_IngestedDataRows_IngestionHistoryId 
    ON IngestedDataRows(IngestionHistoryId);