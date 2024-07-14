import requests
from bs4 import BeautifulSoup

url = 'https://cookierunbraverse.com/cardList'

headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'
}

response = requests.get(url, headers=headers)
soup = BeautifulSoup(response.text, 'html.parser')

# 카드 정보 추출 예제
cards = []
card_elements = soup.select('#card-section .one_fifth a')

for card in card_elements:
    card_name = card['href'].split('/')[-1].replace('-', ' ').title()
    card_img_url = card.find('img')['data-src']
    card_number = card_img_url.split('/')[-1].split('.')[0]
    cards.append({'name': card_name, 'number': card_number})

for card in cards:
    print(f"Card Name: {card['name']}, Card Number: {card['number']}")
