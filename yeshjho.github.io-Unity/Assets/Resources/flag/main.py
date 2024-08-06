import requests
from lxml import etree
import re
import csv
import glob
import os


countries = """Afghanistan
Albania
Algeria
Andorra
Angola
Antigua and Barbuda
Argentina
Armenia
Australia
Austria
Azerbaijan
Bahamas, The
Bahrain
Bangladesh
Barbados
Belarus
Belgium
Belize
Benin
Bhutan
Bolivia
Bosnia and Herzegovina
Botswana
Brazil
Brunei
Bulgaria
Burkina Faso
Burundi
Cambodia
Cameroon
Canada
Cape Verde
Central African Republic
Chad
Chile
China
Colombia
Comoros
Congo, Democratic Republic of the
Congo, Republic of the
Costa Rica
Croatia
Cuba
Cyprus
Czech Republic
Denmark
Djibouti
Dominica
Dominican Republic
East Timor
Ecuador
Egypt
El Salvador
Equatorial Guinea
Eritrea
Estonia
Eswatini
Ethiopia
Fiji
Finland
France
Gabon
Gambia, The
Georgia
Germany
Ghana
Greece
Grenada
Guatemala
Guinea
Guinea-Bissau
Guyana
Haiti
Honduras
Hungary
Iceland
India
Indonesia
Iran
Iraq
Ireland
Israel
Italy
Ivory Coast
Jamaica
Japan
Jordan
Kazakhstan
Kenya
Kiribati
Kuwait
Kyrgyzstan
Laos
Latvia
Lebanon
Lesotho
Liberia
Libya
Liechtenstein
Lithuania
Luxembourg
Madagascar
Malawi
Malaysia
Maldives
Mali
Malta
Marshall Islands
Mauritania
Mauritius
Mexico
Micronesia, Federated States of
Moldova
Monaco
Mongolia
Montenegro
Morocco
Mozambique
Myanmar
Namibia
Nauru
Nepal
Netherlands
New Zealand
Nicaragua
Niger
Nigeria
North Korea
North Macedonia
Norway
Oman
Pakistan
Palau
Palestine
Panama
Papua New Guinea
Paraguay
Peru
Philippines
Poland
Portugal
Qatar
Romania
Russia
Rwanda
Saint Kitts and Nevis
Saint Lucia
Saint Vincent and the Grenadines
Samoa
San Marino
São Tomé and Príncipe
Saudi Arabia
Senegal
Serbia
Seychelles
Sierra Leone
Singapore
Slovakia
Slovenia
Solomon Islands
Somalia
South Africa
South Korea
South Sudan
Spain
Sri Lanka
Sudan
Suriname
Sweden
Switzerland
Syria
Tajikistan
Tanzania
Thailand
Togo
Tonga
Trinidad and Tobago
Tunisia
Turkey
Turkmenistan
Tuvalu
Uganda
Ukraine
United Arab Emirates
United Kingdom
United States
Uruguay
Uzbekistan
Vanuatu
Vatican City
Venezuela
Vietnam
Yemen
Zambia
Zimbabwe
Abkhazia
Cook Islands
Kosovo
Niue
Northern Cyprus
Sahrawi Arab Democratic Republic
Somaliland
South Ossetia
Taiwan
Transnistria"""

"""
for country in countries.splitlines():
    # print(country)
    if ',' in country:
        split = country.split(',')
        country = f"{split[1].strip()} {split[0]}"
    country = country.replace(' ', '_')
    url = f"https://en.wikipedia.org/wiki/{country}#/media/File:Flag_of_{country}.svg"
    # print(url)

    try:
        page = requests.get(url)
        tree = etree.HTML(page.text)
        element = tree.xpath('/html/body/script')
        content = etree.tostring(element[1]).decode('utf-8')
        # print(content)
        pat = re.compile(r'"image":"(.+?)"')
        svg_url = re.findall(pat, content)[0].replace('\\/', '/')
        # print(svg_url)
    except BaseException as e:
        print(country)
        # print(page.content.decode('utf-8'))
        print(e)
    print()
"""

"""
for country in countries.splitlines():
    # print(country)
    original_country_name = country
    if ',' in country:
        split = country.split(',')
        country = f"{split[1].strip()} {split[0]}"
    country = country.replace(' ', '_')
    url = f"https://commons.wikimedia.org/wiki/File:Flag_of_{country}.svg"
    # print(url)

    try:
        page = requests.get(url)
        tree = etree.HTML(page.text)
        candidates = tree.xpath('/html/body/div[3]/div[3]/div[5]/div[1]/div/span/a/@href')
        if len(candidates) == 0:
            print(country)
            continue
        png_url = max(candidates, key=lambda x: int(x.split('/')[-1].split('px')[0]))
        with open(f"{original_country_name}.png", 'wb') as f:
            content = requests.get(png_url, headers={
                'User-Agent': 'Mozilla/5'
            }).content
            f.write(content)
    except BaseException as e:
        print(country)
        print(e)
    print()
"""

rows = []

with open(r'C:\Users\yeshj\Desktop\c.csv') as f:
    reader = csv.DictReader(f)
    for row in reader:
        rows.append(row)

    for flag in glob.glob('*.png'):
        country = flag.split('/')[-1].split('.')[0]
        if ',' in country:
            split = country.split(',')
            country = f"{split[1].strip()} {split[0]}"

        for row in rows:
            if row['english_short'] == country:
                os.rename(flag, row['\ufeffiso_3166_code'] + '.png')
                break
        else:
            print(country)
