import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:iter_mobile/models/image_model.dart';

class ImageSlider extends StatefulWidget {
  final List<ImageModel> images;

  const ImageSlider({Key? key, required this.images}) : super(key: key);

  @override
  State<ImageSlider> createState() => _ImageSliderState();
}

class _ImageSliderState extends State<ImageSlider> {
  final CarouselController _controller = CarouselController();
  int _current = 0;

  @override
  Widget build(BuildContext context) {
    return Stack(
      alignment: Alignment.bottomCenter,
      children: [
        CarouselSlider(
          items: widget.images.asMap().entries.map((image) {
            return GestureDetector(
              onTap: () {
                Navigator.of(context).push(MaterialPageRoute(
                  builder: (_) => FullScreenImage(
                      images: widget.images, initialIndex: image.key),
                ));
              },
              child: Container(
                width: MediaQuery.of(context).size.width,
                child:
                    Image.memory(base64Decode(image.value.image), fit: BoxFit.cover),
              ),
            );
          }).toList(),
          carouselController: _controller,
          options: CarouselOptions(
            height: MediaQuery.of(context).size.height * 0.4,
            autoPlay: false,
            enlargeCenterPage: true,
            viewportFraction: 1.0,
            aspectRatio: 16 / 9,
            onPageChanged: (index, reason) {
              setState(() {
                _current = index;
              });
            },
          ),
        ),
        Positioned(
          bottom: 10, // Adjust the position to your liking
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: widget.images.asMap().entries.map((entry) {
              return GestureDetector(
                onTap: () => _controller.animateToPage(entry.key),
                child: Container(
                  width: 12.0,
                  height: 12.0,
                  margin: const EdgeInsets.symmetric(
                      vertical: 8.0, horizontal: 4.0),
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: (_current == entry.key
                            ? Theme.of(context).primaryColor
                            : Colors.grey)
                        .withOpacity(_current == entry.key ? 0.9 : 0.5),
                  ),
                ),
              );
            }).toList(),
          ),
        ),
      ],
    );
  }
}

class FullScreenImage extends StatefulWidget {
  final List<ImageModel> images;
  final int initialIndex;

  const FullScreenImage(
      {Key? key, required this.images, required this.initialIndex})
      : super(key: key);

  @override
  _FullScreenImageState createState() => _FullScreenImageState();
}

class _FullScreenImageState extends State<FullScreenImage> {
  late CarouselController _controller;
  late int _current;

  @override
  void initState() {
    super.initState();
    _controller = CarouselController();
    _current = widget.initialIndex;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(),
      backgroundColor:
          Colors.black,
      body: Column(
        children: [
          Expanded(
            child: CarouselSlider(
              items: widget.images.map((image) {
                return SizedBox(
                  child: Center(
                    child: Image.memory(base64Decode(image.image),
                        fit: BoxFit.contain),
                  ),
                );
              }).toList(),
              carouselController: _controller,
              options: CarouselOptions(
                initialPage: _current,
                height: MediaQuery.of(context).size.height * 0.5,
                autoPlay: false,
                enlargeCenterPage: true,
                viewportFraction: 1.0,
                aspectRatio: 16 / 9,
                onPageChanged: (index, reason) {
                  setState(() {
                    _current = index;
                  });
                },
              ),
            ),
          ),
          Container(
              padding: EdgeInsets.symmetric(vertical: 10),
              child: Text(
                "${_current + 1}/${widget.images.length}",
                style: TextStyle(color: Colors.white, fontSize: 16),
              ),
            )
        ],
      )
    );
  }
}
