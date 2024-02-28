import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:ui/models/image_model.dart';

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
    return Column(
      children: [
        Stack(
          children: [
            SizedBox(
              width:  MediaQuery.of(context).size.width * 0.9,
              child: CarouselSlider(
                items: widget.images.map((image) {
                  return Builder(
                    builder: (BuildContext context) {
                      return Container(
                        width: MediaQuery.of(context).size.width,
                        margin: const EdgeInsets.symmetric(horizontal: 5.0),
                        child: Image.memory(base64Decode(image.image),
                            fit: BoxFit.cover),
                      );
                    },
                  );
                }).toList(),
                carouselController: _controller,
                options: CarouselOptions(
                  height: MediaQuery.of(context).size.height * 0.6,
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
            Positioned.fill(
              child: Align(
                alignment: Alignment.centerLeft,
                child: GestureDetector(
                  onTap: () => _controller.previousPage(
                    duration: const Duration(milliseconds: 400),
                    curve: Curves.linear,
                  ),
                  child: MouseRegion(
                    cursor: SystemMouseCursors.click,
                    child: Container(
                      padding: EdgeInsets.fromLTRB(30, 0, 0, 0),
                      color: Colors.transparent,
                      alignment: Alignment.centerLeft,
                      child:
                          MouseRegion(cursor: SystemMouseCursors.click, child: const Icon(Icons.arrow_back_ios, color: Colors.white, size: 48)),
                    ),
                  ),
                ),
              ),
            ),
            Positioned.fill(
              child: Align(
                alignment: Alignment.centerRight,
                child: GestureDetector(
                  onTap: () => _controller.nextPage(
                    duration: const Duration(milliseconds: 400),
                    curve: Curves.linear,
                  ),
                  child: MouseRegion(
                    cursor: SystemMouseCursors.click,
                    child: Container(
                      padding: EdgeInsets.fromLTRB(0, 0, 30, 0),
                      color: Colors.transparent,
                      alignment: Alignment.centerRight,
                      child: const Icon(Icons.arrow_forward_ios,
                          color: Colors.white, size: 48),
                    ),
                  ),
                ),
              ),
            )
          ],
        ),
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: widget.images.asMap().entries.map((entry) {
              return GestureDetector(
                onTap: () => _controller.animateToPage(entry.key),
                child: MouseRegion(
                  cursor: SystemMouseCursors.click,
                  child: Container(
                    width: 12.0,
                    height: 12.0,
                    margin: const EdgeInsets.symmetric(
                        vertical: 8.0, horizontal: 4.0),
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: (Theme.of(context).brightness == Brightness.dark
                              ? Colors.white
                              : Colors.black)
                          .withOpacity(_current == entry.key ? 0.9 : 0.4),
                    ),
                  ),
                ),
              );
            }).toList(),
          ),
      ],
    );
  }
}
