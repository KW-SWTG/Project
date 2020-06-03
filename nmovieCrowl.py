import requests
from bs4 import BeautifulSoup
import json
from operator import itemgetter
import datetime
from datetime import datetime, timedelta
import random


def CrwolMovieListByDate(date):
    tagli = []
    for k in range(20):
        tag = "&tg="+str(k)
        tagli.append(tag)
    tagli.reverse()
    tagli.pop()
    count = 0
    MovieDic = []
    MovieNameli = []
    for t in tagli:
        temphtml = requests.get(
            "https://movie.naver.com/movie/sdb/rank/rmovie.nhn?sel=cnt&date="+date+t)
        tempSoup = BeautifulSoup(temphtml.content, 'html.parser')
        MovielstFrame = tempSoup.findAll('tbody')
        Movielst = MovielstFrame[0].findAll('tr')
        for i in Movielst:
            if str(type(i.find("a"))) == "<class 'NoneType'>":
                continue
            count += 1
            MovieName = i.find("a").text
            MRUrl = i.find("a")['href'].split("=")[1]
            if MovieName not in MovieNameli:
                MovieNameli.append(MovieName)
                MovieDic.append(
                    {"MovieName": MovieName, "Url": "https://movie.naver.com/movie/bi/mi/basic.nhn?code="+MRUrl})
    return MovieDic


testday = CrwolMovieListByDate("20050207")
len(testday)
testday


def TopliGenreAdd(topli):
    step = 0
    for i in topli:
        temphtml = requests.get(i['Url'])
        tempSoup = BeautifulSoup(temphtml.content, 'html.parser')
        infoFrame = tempSoup.find("dl", class_="info_spec")
        if infoFrame == None:
            i['genre'] = []
            step += 1
            continue
        infoTag = infoFrame.find("span")
        genreli = []
        for k in infoTag.findAll("a"):
            genreli.append(k.text)
        i['genre'] = genreli
        step += 1
        if step % 10 == 0:
            print(step)


# 영화별 사이트 크롤링 이후 장르 수집 1900개?
TopliGenreAdd(Movie)


def CollectMoPo(topli):
    step = 0
    for i in topli:
        temphtml = requests.get(i['Url'])
        tempSoup = BeautifulSoup(temphtml.content, 'html.parser')
        try:
            PosterUrl = tempSoup.findAll("div", class_="poster")[
                1].find("img")['src']
            i['ImageUrl'] = PosterUrl
        except:
            PosterUrl = "None"
            i['ImageUrl'] = PosterUrl
        try:

        step += 1
        if step % 10 == 0:
            print(step)


def CollectActorAndCountry(topli):
    step = 0
    for i in topli:
        temphtml = requests.get(i['Url'])
        tempSoup = BeautifulSoup(temphtml.content, 'html.parser')
        i['Actor'] = []
        i['Country'] = []
        try:
            ActSoup = tempSoup.findAll("dd")
            for k in ActSoup[1].findAll("a"):
                i['Actor'].append(k.text.split("(")[0])
        except:
            i['Actor'] = ["None"]
        try:
            for t in tempSoup.findAll("dl", class_="info_spec")[0].findAll("a"):
                if("nation" in t['href']):
                    i['Country'].append(t.text)
        except:
            i['Country'] = ["None"]
        step += 1
        if step % 10 == 0:
            print(step)


testhtml = requests.get(Mlist1[4]['Url'])
tempSoup = BeautifulSoup(testhtml.content, 'html.parser')
"nation" in tempSoup.findAll("dl", class_="info_spec")[
    0].findAll("a")[3].text


CollectActorAndCountry(Mlist1)

Mlist1[0:10]


CollectMoPo(Movie)


def DecadeMlist():
    CurTime = datetime.now()
    # DecadeMonth = 12 * 10
    test = 120
    PiriortyTime = CurTime
    DecadeMli = []
    name = []
    # 10년은 4주 * 12달 * 10년
    for week in range(test):
        PiriortyTime = PiriortyTime + timedelta(weeks=-4)
        Timestr = str(PiriortyTime)[0:10].replace("-", "")
        print(PiriortyTime)
        Movieli = CrwolMovieListByDate(Timestr)
        for m in Movieli:
            if m['MovieName'] not in name:
                name.append(m['MovieName'])
                DecadeMli.append(m)
    return DecadeMli


Movie = DecadeMlist()

len(Movie)
with open("Movieli.json", "w", encoding='utf8') as j:
    json.dump(Movie, j, indent=4, ensure_ascii=False)

with open("TopMovieli.json", "r", encoding='utf8') as j:
    Mlist1 = json.load(j)
Mlist1
Movie = sorted(Movie, key=itemgetter('MovieName'))
Movie.reverse()
Movie[0:10]

if __name__ == "__main__":
