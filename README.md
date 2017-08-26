This tool can be used to create parts of **Cool Wallets**, and later to recover them.

# Usage
To **generate** parts for a Private Key, the `-g` argument must be provided, followed by the number of parts required to recover the Private Key, then the total of parts to be generated, and finally the Private Key, between double quotation marks. All arguments must be separated by spaces.

In this example, a 3 / 9 wallet is created. The tool will generate a total of 9 parts, but later only any 3 different generated parts are required to recover the Private Key. 

```
tool -g 3 9 "ThisIsAStrangePrivateKey"
```

Generated parts will be shown in console, using the Short Notation:
```
Parts generated: 
 - 1|3|9|af1-1-622105eff2e34a57e6af256898e508c8f823dc3492ef48a0
 - 1|3|9|561-2-3466980b69178b3c940d455586baa56243fa109af06eaaac
 - 1|3|9|6b1-3-91969eabcbfb1c448e246f279480724adec79fbba280cfeb
 - 1|3|9|1f1-4-c1e06dd2d13a92845ef4e128325679a85aaba33df61317aa
 - 1|3|9|be1-5-64106b7273d605fc44ddcb5a206cae80c7962c1ca4fd72ff
 - 1|3|9|7f1-6-3257f696e822c497367fab673e33032a7c4fe0b2c67c90d7
 - 1|3|9|b41-7-97a7f0364ace53ef2c5681152c09d402e1726f939492f584
 - 1|3|9|581-8-4ecabe239e72cc4d75f239cd7ac1de41d3806f9cf7118a66
 - 1|3|9|db1-9-eb3ab8833c9e5b356fdb13bf68fb09694ebde0bda5ffef6f
```
Note: Even if you use the same parameters to generate the wallet, the generated parts are different and will not be compatible with the parts in this example. If mixed, the resulting Private Key will be unpredictable.

You can also save the output in a file.

```
tool -g 3 9 "ThisIsAStrangePrivateKey" > output.txt
```

Keep in mind that no guarantee is provided with this tool. Use at your own risk.

To **recover** the Private Key, the tool must have the flag argument of `-r`, followed by a list of parts required to recover the Private Key. Parts must be provided in double quotation marks. All arguments (including the parts) must be separated by two or more spaces.

```
tool -r "1|3|9|db1-9-eb3ab8833c9e5b356fdb13bf68fb09694ebde0bda5ffef6f" "1|3|9|1f1-4-c1e06dd2d13a92845ef4e128325679a85aaba33df61317aa" "1|3|9|561-2-3466980b69178b3c940d455586baa56243fa109af06eaaac"
```

If the parts are unit, uncorrupted and compatible with each other, the Private Key will be shown in console:
```
Recovered Private Key:
  ThisIsAStrangePrivateKey
```

To get some **help**, you can use the `-h` argument.
```
tool -h
```

# What is a Cool Wallet?
The theoretical definition of a Cool Wallet is _a compound wallet of m total parts, in a way that a quantity under n parts is not enough to recover the private key, but on or over n parts is, assuming that n is less or equal to m._ Another name for a Cool Wallet is a [Multi-signature wallet](https://en.bitcoin.it/wiki/Multisignature), but often its use case is more technical or internal than that of a Cool Wallet.

Maybe an example is a better way to explain what a Cool Wallet is.

Suppose Mark wants to mitigate the risk of storing the 12 BTC he saved growing up. He first considered a Cold Wallet, but he also likes to travel, so the media can be lost or stolen. He still wants to have his funds in hand for any emergency. He realizes that a Cold Wallet is not ideal for his purposes, so he considered a Cool Wallet instead. He generated a simple Public-Private key pair and provided the Private Key to a Cold Wallet tool to generate the parts of a Cool Wallet. He generated 6 parts in a way that only 3 different parts are required to recover the Private Key and get all his funds, which means he created a 3 / 6 Cool Wallet.

Mark stored one wallet part in his mother's house (1), he gave another to a trusted friend (2), he stored another one in his laptop (3, remember it is not a Cold Wallet), another one in his physical wallet as a QR code (4), he stored another one in his USB drive (5) and the last one is stored as an image in his cell phone (6). Mark may lost his wallet, phone and wife (just kidding), but he will still able to recover the funds from the parts left and create a new Cool Wallet to move the funds from the previous one. A very clever hacker could had access to the Cool Wallet's part stored in Mark's laptop, but the hacker was unable to get any funds without more different parts to combine.

Mark never lost control of his wallet, since only he knew and had access to all his Cool Wallet's parts. He also mitigated the risk of losing his wallet, since only a fraction of the generated parts are required to recover the Private Key.

Therefore, Cool Wallets are called as such because they may or may not be strictly used as Cold Wallets, but they are a lot safer than online wallets or exchanges as storage of cryptocurrencies.
