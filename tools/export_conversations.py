import json

import gspread

from gspread.client import BackoffClient

SPREADSHEET_ID = "1cNrfg4Ku5uxIjxssHnfXP516UKGucI6p6XKgAJ9eyLY"
WORKSHEETS_TO_IGNORE = ["Question Ideas"]

if __name__ == "__main__":
    gc = gspread.service_account(client_factory=BackoffClient)

    spreadsheet = gc.open_by_key(SPREADSHEET_ID)

    worksheets = spreadsheet.worksheets()

    encounters = {"encounters": []}

    for idx, worksheet in enumerate(worksheets):
        if worksheet.title in WORKSHEETS_TO_IGNORE:
            continue

        encounter_obj = {"name": worksheet.title, "encounterRounds": []}

        questions_column = worksheet.col_values(1)

        for start_row_index, question_text in enumerate(questions_column):
            if question_text == "":
                continue

            question_obj = {"question": question_text, "responses": {}}

            for row_index in range(start_row_index + 1, start_row_index + 4):
                row_data = worksheet.row_values(row_index)
                quality_descriptor = row_data[1].lower()
                responses = row_data[2:]
                question_obj["responses"][quality_descriptor] = responses

            encounter_obj["encounterRounds"].append(question_obj)

        encounters["encounters"].append(encounter_obj)

    with open("question-data.json", "w") as fh:
        json.dump(encounters, fh, indent=4)
