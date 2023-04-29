import json

import gspread

SPREADSHEET_ID = "1cNrfg4Ku5uxIjxssHnfXP516UKGucI6p6XKgAJ9eyLY"
WORKSHEETS_TO_IGNORE = ["Question Ideas"]

if __name__ == "__main__":
    gc = gspread.service_account()

    spreadsheet = gc.open_by_key(SPREADSHEET_ID)

    worksheets = spreadsheet.worksheets()

    scenarios = {}

    for worksheet in worksheets:
        if worksheet.title in WORKSHEETS_TO_IGNORE:
            continue

        scenarios[worksheet.title] = []

        questions_column = worksheet.col_values(1)

        for start_row_index, question_text in enumerate(questions_column):
            if question_text == "":
                continue

            question_obj = {question_text: {}}

            for row_index in range(start_row_index + 1, start_row_index + 4):
                row_data = worksheet.row_values(row_index)
                quality_descriptor = row_data[1].lower()
                responses = row_data[2:]

                question_obj[question_text][quality_descriptor] = responses

            scenarios[worksheet.title].append(question_obj)

    with open("question-data.json", "w") as fh:
        json.dump(scenarios, fh, indent=4)
